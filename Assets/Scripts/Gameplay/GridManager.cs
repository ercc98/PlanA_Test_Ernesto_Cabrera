

using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Gameplay;
using NUnit.Framework;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 5;
    private Dictionary<Vector2Int, Block> grid = new();
    private List<Block> deletedBlocks = new();
    public List<Sprite> blockSprites = new List<Sprite>();
    private bool isProcessing = false;
    private int[] blockDeletedPerColumn;
    private IObjectPool<Block> blockPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockDeletedPerColumn = new int[width];
        blockPool = new ObjectPool<Block>(blockPrefab.GetComponent<Block>(), transform, width * height);
        InitializeGrid();
    }

    private Block CreateBlock(Vector2Int position)
    {
        int type = UnityEngine.Random.Range(0, blockSprites.Count);
        Sprite sprite = blockSprites[type];
        Block block = blockPool.GetObject();
        block.Initialize(position, type, sprite);
        block.OnBlockClicked += OnBlockClicked;
        return block;
    }
    
    private void InitializeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int position = new Vector2Int(x, y);                
                Block block = CreateBlock(position);
                grid.Add(position, block);
            }
        }
    }

    private void OnBlockClicked(Block block)
    {
        if (isProcessing) return;
        GameController.instance.AddMove();
        isProcessing = true;
        grid.Remove(block.GridPosition);
        deletedBlocks.Add(block);
        blockDeletedPerColumn[block.GridPosition.x]++;
        DFS(block);
        foreach (Block deletedBlock in deletedBlocks)
        {
            GameController.instance.AddScore(10);
            deletedBlock.OnBlockClicked -= OnBlockClicked;
            blockPool.ReturnObject(deletedBlock);
        }
        deletedBlocks.Clear();
        StartCoroutine(FillEmptySpaces());        
    }

    private void DFS(Block startBlock)
    {
        Stack<Block> stack = new Stack<Block>();
        HashSet<Block> visited = new HashSet<Block>();
        stack.Push(startBlock);
        visited.Add(startBlock);

        while (stack.Count > 0)
        {
            Block currentBlock = stack.Pop();
            foreach(Block neighbor in GetNeighbors(currentBlock))
            {
                if (!visited.Contains(neighbor) && neighbor.BlockType == startBlock.BlockType)
                {
                    visited.Add(neighbor);
                    stack.Push(neighbor);
                    grid.Remove(neighbor.GridPosition);
                    deletedBlocks.Add(neighbor);
                    blockDeletedPerColumn[neighbor.GridPosition.x]++;
                }
            }
        }
    }

    private IEnumerable<Block> GetNeighbors(Block currentBlock)
    {
        Vector2Int pos = currentBlock.GridPosition;
        Vector2Int[] directions = new Vector2Int[]
        {
            new(0, 1),
            new(1, 0),
            new(0, -1),
            new(-1, 0)
        };
        foreach (var dir in directions)
        {
            Vector2Int neighborPos = pos + dir;
            if (grid.ContainsKey(neighborPos))
            {
                yield return grid[neighborPos];
            }
        }
    }

    private IEnumerator FillEmptySpaces()
    {
        yield return new WaitForSeconds(1f);
        SpawnNewBlocksPerColumn();
        ShiftBlocksDown();        
        isProcessing = false;

    }
    public void SpawnNewBlocksPerColumn()
    {
        for (int x = 0; x < width; x++)
        {
            int blocksToCreate = blockDeletedPerColumn[x];
            for (int y = 0; y < blocksToCreate; y++)
            {
                Vector2Int position = new Vector2Int(x, height + y);
                Block block = CreateBlock(position);
                grid.Add(position, block);
            }
        }
    }
    
    private void ShiftBlocksDown()
    {
        for (int x = 0; x < width; x++)
        {
            int shift = 0;
            for (int y = 0; y < height + blockDeletedPerColumn[x]; y++)
            {
                Vector2Int position = new(x, y);
                if (!grid.ContainsKey(position))
                {
                    shift++;
                }
                else if (shift > 0)
                {
                    Block block = grid[position];
                    grid.Remove(position);
                    Vector2Int newPosition = new(x, y - shift);
                    block.SetGridPosition(newPosition);
                    block.transform.position = (Vector2)newPosition;
                    grid.Add(newPosition, block);
                }
            }
            blockDeletedPerColumn[x] = 0;
        }
        isProcessing = false;
    }
}
