using System.Collections;
using UnityEngine;

// Moves ONE parent object (BoardsRoot) like a camera: zoomed out to see all 9 boards,
// or zoomed in so a single board fills the screen. The boards themselves are never touched.
//
// Requirements in the scene:
//  - _boardsRoot: centered anchors and pivot, sitting at the middle of the canvas.
//  - Every board is a DIRECT child of _boardsRoot with centered anchors and pivot.
//  - The middle board (board 5) sits at (0, 0) inside the root, so the grid is centered.
public class BoardsView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _boardsRoot;
    [Tooltip("The 9 board RectTransforms in play order (top row left to right, then middle, then bottom).")]
    [SerializeField] private RectTransform[] _boardRects;

    [Header("Focus offsets")]
    [Tooltip("One per board, same order as Board Rects. Starts from the board's own RectTransform: (0, 0) centers the focused board on screen. " +
         "Positive X shifts the focused board right on screen, positive Y shifts it up. Units are the same as the board's position. " +
         "Missing entries count as (0, 0).")]
    [SerializeField] private Vector2[] _focusOffsets = new Vector2[9];


    [Header("Zoom")]
    [Tooltip("Scale of BoardsRoot when all 9 boards are visible. Roughly: smaller screen side / (3 * boardSize + 2 * gap).")]
    [SerializeField, Range(0.05f, 1f)] private float _overviewScale = 0.33f;
    [SerializeField] private float _zoomDuration = 0.5f;
    [SerializeField] private AnimationCurve _ease = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Testing")]
    [SerializeField, Range(0, 8)] private int _testBoardIndex;

    private const float FocusScale = 1f;

    // animated (use with StartCoroutine or yield return)

    public IEnumerator ZoomToBoard(int index)
    {
        return AnimateTo(FocusScale, PositionForBoard(index, FocusScale));
    }

    public IEnumerator ZoomToOverview()
    {
        return AnimateTo(_overviewScale, Vector2.zero);
    }

    // instant

    public void SnapToBoard(int index)
    {
        Apply(FocusScale, PositionForBoard(index, FocusScale));
    }

    public void SnapToOverview()
    {
        Apply(_overviewScale, Vector2.zero);
    }

    // internals

    // To put a board in the middle of the screen, shift the root the opposite way
    // by that board's offset (scaled, because the root's scale multiplies the offset).
    private Vector2 PositionForBoard(int index, float scale)
    {
        return (-_boardRects[index].anchoredPosition + OffsetFor(index)) * scale;
    }

    private Vector2 OffsetFor(int index)
    {
        if (_focusOffsets == null || index < 0 || index >= _focusOffsets.Length) return Vector2.zero;
        return _focusOffsets[index];
    }


    private void Apply(float scale, Vector2 position)
    {
        _boardsRoot.localScale = Vector3.one * scale;
        _boardsRoot.anchoredPosition = position;
    }

    private IEnumerator AnimateTo(float targetScale, Vector2 targetPosition)
    {
        float startScale = _boardsRoot.localScale.x;
        Vector2 startPosition = _boardsRoot.anchoredPosition;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, _zoomDuration);
            float k = _ease.Evaluate(Mathf.Clamp01(t));

            Apply(Mathf.Lerp(startScale, targetScale, k), Vector2.Lerp(startPosition, targetPosition, k));
            yield return null;
        }

        Apply(targetScale, targetPosition);   // land exactly on the target
    }

    // testing (right-click the component header)

    [ContextMenu("Test: Snap To Overview")]
    private void TestSnapToOverview()
    {
        SnapToOverview();
    }

    [ContextMenu("Test: Snap To Test Board")]
    private void TestSnapToTestBoard()
    {
        SnapToBoard(_testBoardIndex);
    }

    [ContextMenu("Test: Zoom To Test Board (Play mode)")]
    private void TestZoomToTestBoard()
    {
        if (Application.isPlaying) StartCoroutine(ZoomToBoard(_testBoardIndex));
    }

    [ContextMenu("Test: Zoom To Overview (Play mode)")]
    private void TestZoomToOverview()
    {
        if (Application.isPlaying) StartCoroutine(ZoomToOverview());
    }
}