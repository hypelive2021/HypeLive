using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Graphic �ł���������
/// ���_�̏��Ԃ����̂ɂ���ĈقȂ�̂�Image����
/// </summary>
[RequireComponent(typeof(Image))]
public class CurveImage : BaseMeshEffect
{
    [SerializeField, Range(1, 50)]
    private int _divide = 10;
    [SerializeField]
    private float _radius = 1f;
    [SerializeField, Range(0, 180f)]
    private float _outerLength = 10;

    private readonly List<UIVertex> _vertexList = new List<UIVertex>();

    private float _posMin;
    private float _posMax;
    private float _uvMin;
    private float _uvMax;
    private Vector2 _size;

    public override void ModifyMesh(VertexHelper helper)
    {
        if (_divide <= 0)
            return;

        _vertexList.Clear();
        helper.GetUIVertexStream(_vertexList);

        var rect = transform as RectTransform;
        _size = new Vector2(rect.rect.width, rect.rect.height);

        // ���������_�𕡐�
        for (var i = 0; i < _divide; i++)
        {
            for (var j = 0; j < 6; j++)
            {
                _vertexList.Add(_vertexList[j]);
            }
        }

        _posMin = _vertexList.Min(v => v.position.x);
        _posMax = _vertexList.Max(v => v.position.x);
        _uvMin = _vertexList.Min(v => v.uv0.x);
        _uvMax = _vertexList.Max(v => v.uv0.x);
        SetMeshesRect();
        SetMeshesCurve();
        helper.Clear();
        helper.AddUIVertexTriangleStream(_vertexList);
    }

    /// <summary>
    /// �S���_�𒷕��`�ɔz�u
    /// </summary>
    private void SetMeshesRect()
    {
        // �������̋���
        var offset = (_posMax - _posMin) / (_divide + 1);
        var x = _posMin;
        // ���_�̈ʒu��ϓ�
        for (var i = 0; i < _vertexList.Count; i += 6)
        {
            var t = Mathf.InverseLerp(_posMin, _posMax, x);
            // ����
            foreach (var index in new[] { 0, 1, 5 })
            {
                SetMeshRect(i + index, t);
            }
            x += offset;
            t = Mathf.InverseLerp(_posMin, _posMax, x);
            // �E��
            foreach (var index in new[] { 2, 3, 4 })
            {
                SetMeshRect(i + index, t);
            }
        }
    }

    /// <summary>
    /// ���b�V���𒷕��`�ɔz�u
    /// </summary>
    private void SetMeshRect(int index, float t)
    {
        var vertex = _vertexList[index];
        var p = vertex.position;
        p.x = Mathf.Lerp(_posMin, _posMax, t);
        vertex.position = p;
        var uv0 = vertex.uv0;
        uv0.x = Mathf.Lerp(_uvMin, _uvMax, t);
        vertex.uv0 = uv0;
        _vertexList[index] = vertex;
    }

    /// <summary>
    /// �����`�ɔz�u�������_���J�[�u������
    /// </summary>
    private void SetMeshesCurve()
    {
        for (var i = 0; i < _vertexList.Count; i += 6)
        {
            // ��
            foreach (var index in new[] { 0, 4, 5 })
                SetMeshCurve(i + index, false);
            // ��
            foreach (var index in new[] { 1, 2, 3 })
                SetMeshCurve(i + index, true);
        }
    }

    /// <summary>
    /// �����`�̔z�u�͏I������̂ŁA�J�[�u�̔z�u�ݒ�
    /// </summary>
    private void SetMeshCurve(int index, bool isTop)
    {
        var vertex = _vertexList[index];
        var p = vertex.position;

        var t = Mathf.InverseLerp(_posMin, _posMax, p.x);
        var angle = 90 + Mathf.Lerp(_outerLength / 2f, -_outerLength / 2f, t);

        // �㕔��x�����炷�K�v������
        if (isTop)
        {
            var radius = _radius + _size.y;
            // ��
            var bottom = GetCirclePosition(angle, _radius);
            // ��
            var top = GetCirclePosition(angle, radius);
            // �����������ړ�������
            p.x -= bottom.x - top.x;
            p.y = p.y - radius + top.y;
        }
        else
        {
            var pos = GetCirclePosition(angle, _radius);
            p.y = p.y - _radius + pos.y;
        }

        vertex.position = p;
        _vertexList[index] = vertex;
    }

    /// <summary>
    /// ���͂̈ʒu���擾
    /// </summary>
    private Vector2 GetCirclePosition(float angle, float radius)
    {
        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );
    }
}