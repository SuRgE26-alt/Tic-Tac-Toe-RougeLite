using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// A transparent layer inside BoardsRoot, drawn on top of the boards.
// Draws one line per seam line and pulses its 3 cells. Lines stay until Clear() is called.
public class SeamLineOverlay : MonoBehaviour
{
    [Header("Look (units are BoardsRoot units, so they shrink with the overview zoom)")]
    [SerializeField] private float _thickness = 30f;
    [Tooltip("How far the line extends past the centers of the first and last cell.")]
    [SerializeField] private float _endPadding = 60f;
    [SerializeField] private Color _xColor = new Color(0.95f, 0.35f, 0.35f, 0.9f);
    [SerializeField] private Color _oColor = new Color(0.35f, 0.65f, 0.95f, 0.9f);

    [Header("Timing")]
    [SerializeField] private float _drawDuration = 0.35f;
    [SerializeField] private float _flashDuration = 0.35f;
    [SerializeField] private float _flashScale = 1.35f;

    private RectTransform _rect;
    private RectTransform Rect
    {
        get
        {
            if (_rect == null) _rect = (RectTransform)transform;
            return _rect;
        }
    }

    // Original scale of every cell we have pulsed, so Clear() can put them back.
    private readonly Dictionary<Transform, Vector3> _pulsedCells = new Dictionary<Transform, Vector3>();

    // Run with "yield return" from the controller (so StopAllCoroutines there cancels it).
    public IEnumerator Reveal(string owner, TextMeshProUGUI[] cells)
    {
        Vector2 first = ToLocal(cells[0]);
        Vector2 last = ToLocal(cells[cells.Length - 1]);

        Vector2 direction = (last - first).normalized;
        Vector2 start = first - direction * _endPadding;
        float length = Vector2.Distance(first, last) + 2f * _endPadding;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        RectTransform line = CreateLine(owner, start, angle);

        // 1. the line grows from the first cell to the last
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, _drawDuration);
            line.sizeDelta = new Vector2(length * Mathf.Clamp01(t), _thickness);
            yield return null;
        }
        line.sizeDelta = new Vector2(length, _thickness);

        // 2. the three cells pulse
        foreach (TextMeshProUGUI cell in cells)
        {
            if (!_pulsedCells.ContainsKey(cell.transform))
            {
                _pulsedCells.Add(cell.transform, cell.transform.localScale);
            }
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, _flashDuration);
            float pulse = Mathf.Sin(Mathf.Clamp01(t) * Mathf.PI);          // 0 -> 1 -> 0

            foreach (TextMeshProUGUI cell in cells)
            {
                Vector3 original = _pulsedCells[cell.transform];
                cell.transform.localScale = original * Mathf.Lerp(1f, _flashScale, pulse);
            }
            yield return null;
        }

        foreach (TextMeshProUGUI cell in cells)
        {
            cell.transform.localScale = _pulsedCells[cell.transform];
        }
    }

    // Removes every line and restores any cell left mid-pulse (used by restart).
    public void Clear()
    {
        for (int i = Rect.childCount - 1; i >= 0; i--)
        {
            Destroy(Rect.GetChild(i).gameObject);
        }

        foreach (KeyValuePair<Transform, Vector3> pair in _pulsedCells)
        {
            if (pair.Key != null) pair.Key.localScale = pair.Value;
        }
        _pulsedCells.Clear();
    }

    private RectTransform CreateLine(string owner, Vector2 start, float angle)
    {
        GameObject go = new GameObject("SeamLine", typeof(RectTransform), typeof(Image));

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(Rect, false);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0f, 0.5f);                    // grows out of its left end
        rt.sizeDelta = new Vector2(0f, _thickness);
        rt.localPosition = new Vector3(start.x, start.y, 0f);
        rt.localRotation = Quaternion.Euler(0f, 0f, angle);

        Image image = go.GetComponent<Image>();
        image.color = (owner == "X") ? _xColor : _oColor;
        image.raycastTarget = false;                         // never block clicks

        return rt;
    }

    // The center of a cell, expressed in this overlay's own local space.
    private Vector2 ToLocal(TextMeshProUGUI cell)
    {
        RectTransform r = cell.rectTransform;
        Vector3 world = r.TransformPoint(r.rect.center);
        return Rect.InverseTransformPoint(world);
    }
}