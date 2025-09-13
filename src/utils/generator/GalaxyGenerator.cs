namespace Nanomachine;

using System;
using System.Text.Json.Nodes;

public static class GalaxyGenerator {
    public static JsonObject RandomGalaxy(int seed, JsonObject parameters) {
        // 从参数表中提取所需参数
        var maxX = parameters["maxX"]?.GetValue<float>();
        var maxY = parameters["maxY"]?.GetValue<float>();
        var zRange = parameters["zRange"]?.GetValue<float>();
        var n = (parameters["n"]?.GetValue<int>()) ?? 0;

        // 将种子用于初始化随机数生成器
        var random = new Random(seed);

        // 创建结果字典
        var result = new JsonObject {
            ["seed"] = seed,
            ["parameters"] = parameters
        };

        // 生成坐标列表
        var coordinates = new JsonArray();

        for (var i = 0; i < n; i++) {
            // 生成在[-maxX, maxX]和[-maxY, maxY]范围内的坐标
            var x = ((random.NextDouble() * 2) - 1) * (maxX ?? 20);
            var y = ((random.NextDouble() * 2) - 1) * (maxY ?? 20);
            // Z坐标在[-zRange, zRange]范围内
            var z = ((random.NextDouble() * 2) - 1) * (zRange ?? 2);

            // 创建坐标字典
            var coordinate = new JsonObject {
                ["x"] = (float)Math.Round(x, 2),
                ["y"] = (float)Math.Round(y, 2),
                ["z"] = (float)Math.Round(z, 2),
                ["id"] = i + 1,
                // 添加附加参数（类型）
                ["type"] = (float)random.NextDouble()
            };

            coordinates.Add(coordinate);
        }

        // 将坐标列表添加到结果中
        result.Add("stars", coordinates);
        result.Add("generatedTime", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        return result;
    }
}