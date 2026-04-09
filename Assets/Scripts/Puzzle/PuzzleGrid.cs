using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGrid : MonoBehaviour
{
    [Header("조각 프리팹")]
    public GameObject piecePrefab;

    [Header("그리드 설정")]
    public float cellSize = 160f;
    public float spacing = 3f;

    // 알츠하이머 전용: 프레임 크기 제한
    [HideInInspector] public int maxPieces = -1;

    readonly Dictionary<string, MemoryPiece> pieceMap = new Dictionary<string, MemoryPiece>();
    readonly List<MemoryPiece> allPieces = new List<MemoryPiece>();

    public IReadOnlyList<MemoryPiece> AllPieces => allPieces;

    public void Build(PatientData patient)
    {
        pieceMap.Clear();
        allPieces.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        var piecesData = patient.pieces;
        var gridSize   = patient.gridSize;
        var texture    = patient.memoryImage;
        var altTexture = patient.memoryImageAlt;

        // GridLayoutGroup 설정
        var layout = GetComponent<GridLayoutGroup>();
        if (layout == null) layout = gameObject.AddComponent<GridLayoutGroup>();
        layout.cellSize        = new Vector2(cellSize, cellSize);
        layout.spacing         = new Vector2(spacing, spacing);
        layout.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = gridSize.x;
        layout.startCorner     = GridLayoutGroup.Corner.UpperLeft;
        layout.startAxis       = GridLayoutGroup.Axis.Horizontal;

        // row → col 순으로 정렬
        var sorted = new List<MemoryPieceData>(piecesData);
        sorted.Sort((a, b) =>
        {
            if (a.gridPosition.y != b.gridPosition.y)
                return a.gridPosition.y.CompareTo(b.gridPosition.y);
            return a.gridPosition.x.CompareTo(b.gridPosition.x);
        });

        foreach (var data in sorted)
        {
            var go    = Instantiate(piecePrefab, transform);
            var piece = go.GetComponent<MemoryPiece>();
            if (piece == null) piece = go.AddComponent<MemoryPiece>();

            piece.Init(data, texture, altTexture, gridSize);

            pieceMap[data.pieceId] = piece;
            allPieces.Add(piece);
        }
    }

    public MemoryPiece GetPiece(string pieceId)
    {
        pieceMap.TryGetValue(pieceId, out var piece);
        return piece;
    }

    public void FadeConnectedPieces(string[] connectedIds)
    {
        foreach (var id in connectedIds)
        {
            var piece = GetPiece(id);
            if (piece != null && !piece.IsProcessed)
                piece.ApplyFadeEffect();
        }
    }

    // 알츠하이머: OptionA(남긴다) 가능 여부
    public bool CanKeep()
    {
        if (maxPieces < 0) return true;
        int keptCount = 0;
        foreach (var p in allPieces)
            if (p.ProcessState == PieceProcessState.OptionA) keptCount++;
        return keptCount < maxPieces;
    }

    public bool AllProcessed()
    {
        foreach (var p in allPieces)
            if (!p.IsProcessed) return false;
        return true;
    }
}
