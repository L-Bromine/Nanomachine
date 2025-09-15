namespace Nanomachine;

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;



/// <summary>
/// 用于处理 JSON 对象文件的读取和保存操作
/// </summary>
public class FileU {
    /// <summary>
    /// 从指定路径读取 JSON 文件并转换为 JsonObject
    /// </summary>
    /// <param name="filePath">JSON 文件路径</param>
    /// <returns>读取的 JsonObject 对象</returns>
    public static async Task<JsonObject> ReadJsonFromFileAsync(string filePath) {
        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException($"找不到指定的 JSON 文件: {filePath}");
        }

        try {
            await using var fileStream = File.OpenRead(filePath);
            var jsonNode = await JsonNode.ParseAsync(fileStream);
            return jsonNode?.AsObject() ?? throw new InvalidOperationException("JSON 文件内容为空或不是有效的 JSON 对象");
        }
        catch (JsonException ex) {
            throw new InvalidOperationException("JSON 文件格式错误", ex);
        }
        catch (Exception ex) {
            throw new IOException($"读取 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 从指定路径读取 JSON 文件并转换为 JsonObject (同步版本)
    /// </summary>
    /// <param name="filePath">JSON 文件路径</param>
    /// <returns>读取的 JsonObject 对象</returns>
    public static JsonObject ReadJsonFromFile(string filePath) {
        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException($"找不到指定的 JSON 文件: {filePath}");
        }

        try {
            var jsonContent = File.ReadAllText(filePath);
            var jsonNode = JsonNode.Parse(jsonContent);
            return jsonNode?.AsObject() ?? throw new InvalidOperationException("JSON 文件内容为空或不是有效的 JSON 对象");
        }
        catch (JsonException ex) {
            throw new InvalidOperationException("JSON 文件格式错误", ex);
        }
        catch (Exception ex) {
            throw new IOException($"读取 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 将 JsonObject 保存到指定路径的 JSON 文件中
    /// </summary>
    /// <param name="jsonObject">要保存的 JsonObject 对象</param>
    /// <param name="filePath">目标文件路径</param>
    /// <param name="options">JSON 序列化选项</param>
    public static async Task SaveJsonToFileAsync(JsonObject jsonObject, string filePath, JsonSerializerOptions? options = null) {
        if (jsonObject == null) {
            throw new ArgumentNullException(nameof(jsonObject), "JSON 对象不能为空");
        }

        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        try {
            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            // 设置默认选项（如果需要）

            options ??= new JsonSerializerOptions { WriteIndented = true };

            await using var fileStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(fileStream, jsonObject, options);
        }
        catch (Exception ex) {
            throw new IOException($"保存 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 将 JsonObject 保存到指定路径的 JSON 文件中 (同步版本)
    /// </summary>
    /// <param name="jsonObject">要保存的 JsonObject 对象</param>
    /// <param name="filePath">目标文件路径</param>
    /// <param name="options">JSON 序列化选项</param>
    public static void SaveJsonToFile(JsonObject jsonObject, string filePath, JsonSerializerOptions? options = null) {
        if (jsonObject == null) {
            throw new ArgumentNullException(nameof(jsonObject), "JSON 对象不能为空");
        }


        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        try {
            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            // 设置默认选项（如果需要）

            options ??= new JsonSerializerOptions { WriteIndented = true };

            var jsonContent = JsonSerializer.Serialize(jsonObject, options);
            File.WriteAllText(filePath, jsonContent);
        }
        catch (Exception ex) {
            throw new IOException($"保存 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 从 JSON 文件读取并反序列化为指定类型的对象
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="filePath">JSON 文件路径</param>
    /// <param name="options">JSON 序列化选项</param>
    /// <returns>反序列化后的对象</returns>
    public static async Task<T?> ReadFromFileAsync<T>(string filePath, JsonSerializerOptions? options = null) {
        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException($"找不到指定的 JSON 文件: {filePath}");
        }

        try {
            await using var fileStream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(fileStream, options);
        }
        catch (JsonException ex) {
            throw new InvalidOperationException("JSON 反序列化错误", ex);
        }
        catch (Exception ex) {
            throw new IOException($"读取 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 从 JSON 文件读取并反序列化为指定类型的对象 (同步版本)
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="filePath">JSON 文件路径</param>
    /// <param name="options">JSON 序列化选项</param>
    /// <returns>反序列化后的对象</returns>
    public static T? ReadFromFile<T>(string filePath, JsonSerializerOptions? options = null) {
        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException($"找不到指定的 JSON 文件: {filePath}");
        }

        try {
            var jsonContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonContent, options);
        }
        catch (JsonException ex) {
            throw new InvalidOperationException("JSON 反序列化错误", ex);
        }
        catch (Exception ex) {
            throw new IOException($"读取 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 将对象序列化为 JSON 并保存到指定文件
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要序列化的对象</param>
    /// <param name="filePath">目标文件路径</param>
    /// <param name="options">JSON 序列化选项</param>
    public static async Task SaveToFileAsync<T>(T obj, string filePath, JsonSerializerOptions? options = null) {
        if (obj == null) {
            throw new ArgumentNullException(nameof(obj), "对象不能为空");
        }

        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        try {
            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            // 设置默认选项（如果需要）

            options ??= new JsonSerializerOptions { WriteIndented = true };

            await using var fileStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(fileStream, obj, options);
        }
        catch (Exception ex) {
            throw new IOException($"保存 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 将对象序列化为 JSON 并保存到指定文件 (同步版本)
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要序列化的对象</param>
    /// <param name="filePath">目标文件路径</param>
    /// <param name="options">JSON 序列化选项</param>
    public static void SaveToFile<T>(T obj, string filePath, JsonSerializerOptions? options = null) {
        if (obj == null) {
            throw new ArgumentNullException(nameof(obj), "对象不能为空");
        }

        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("文件路径不能为空", nameof(filePath));
        }

        try {
            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            // 设置默认选项（如果需要）

            options ??= new JsonSerializerOptions { WriteIndented = true };

            var jsonContent = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filePath, jsonContent);
        }
        catch (Exception ex) {
            throw new IOException($"保存 JSON 文件时发生错误: {ex.Message}", ex);
        }
    }
}