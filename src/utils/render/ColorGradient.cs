namespace Nanomachine.Render;

using Godot;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 颜色渐变生成器
/// </summary>
public class ColorGradient {
    private SortedDictionary<float, Color> _colorStops;

    private const byte MIN_BYTE = 0; // 最小值
    private const byte MAX_BYTE = 255; // 最大值

    /// <summary>
    /// 初始化一个空的颜色渐变
    /// </summary>
    public ColorGradient() {
        _colorStops = [];
    }

    /// <summary>
    /// 使用预定义的颜色锚点初始化颜色渐变
    /// </summary>
    /// <param name="stops">颜色锚点字典，键为位置(0-1)，值为颜色</param>
    public ColorGradient(Dictionary<float, Color> stops) {
        _colorStops = new SortedDictionary<float, Color>(stops);
        ValidateStops();
    }

    /// <summary>
    /// 添加或更新颜色锚点
    /// </summary>
    /// <param name="position">位置(0-1)</param>
    /// <param name="color">颜色</param>
    public void AddColorStop(float position, Color color) {
        if (position is < 0 or > 1) {
            throw new ArgumentException("位置必须在0到1之间");
        }

        if (_colorStops.ContainsKey(position)) {
            _colorStops[position] = color;
        }
        else {
            _colorStops.Add(position, color);
        }
    }

    /// <summary>
    /// 批量设置颜色锚点
    /// </summary>
    /// <param name="stops">颜色锚点字典</param>
    public void SetColorStops(Dictionary<float, Color> stops) {
        _colorStops = new SortedDictionary<float, Color>(stops);
        ValidateStops();
    }

    /// <summary>
    /// 获取所有颜色锚点
    /// </summary>
    /// <returns>颜色锚点字典</returns>
    public Dictionary<float, Color> GetColorStops() {
        return _colorStops.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    /// <summary>
    /// 清除所有颜色锚点
    /// </summary>
    public void ClearColorStops() {
        _colorStops.Clear();
    }

    /// <summary>
    /// 根据位置获取插值后的颜色
    /// </summary>
    /// <param name="position">位置(0-1)</param>
    /// <returns>插值后的颜色</returns>
    public Color GetColor(float position) {
        if (_colorStops.Count == 0) {
            throw new InvalidOperationException("没有设置任何颜色锚点");
        }

        if (position <= 0) {
            return _colorStops.First().Value;
        }

        if (position >= 1) {
            return _colorStops.Last().Value;
        }

        // 找到相邻的两个锚点
        var previous = _colorStops.First();
        var next = _colorStops.Last();

        foreach (var stop in _colorStops) {
            if (stop.Key <= position && stop.Key >= previous.Key) {
                previous = stop;
            }

            if (stop.Key >= position && stop.Key <= next.Key) {
                next = stop;
            }
        }

        // 如果位置正好是某个锚点，直接返回该颜色
        if (Math.Abs(previous.Key - position) < float.Epsilon) {
            return previous.Value;
        }

        if (Math.Abs(next.Key - position) < float.Epsilon) {
            return next.Value;
        }

        // 计算插值比例
        var t = (position - previous.Key) / (next.Key - previous.Key);


        // 对每个颜色分量进行插值
        var r = previous.Value.R + ((next.Value.R - previous.Value.R) * t);
        var g = previous.Value.G + ((next.Value.G - previous.Value.G) * t);
        var b = previous.Value.B + ((next.Value.B - previous.Value.B) * t);
        var a = previous.Value.A + ((next.Value.A - previous.Value.A) * t);

        // 确保值在有效范围内
        r = Math.Max(0, Math.Min(1, r));
        g = Math.Max(0, Math.Min(1, g));
        b = Math.Max(0, Math.Min(1, b));
        a = Math.Max(0, Math.Min(1, a));

        return new Color(r, g, b, a);
    }

    /// <summary>
    /// 验证颜色锚点是否有效
    /// </summary>
    private void ValidateStops() {
        if (_colorStops.Any(stop => stop.Key is < 0 or > 1)) {
            throw new ArgumentException("所有位置必须在0到1之间");
        }
    }
}
