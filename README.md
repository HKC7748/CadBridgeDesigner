# CadBridgeDesigner

一个基于 AutoCAD 的桥梁三维设计插件（.NET / C#），通过参数化方式在 DWG 中创建桥梁构件（柱式墩、T 梁），并提供项目管理器、属性编辑器、日志等 WPF 停靠面板。

## 功能特性

- **参数化建模**
  - 柱式墩（ColumnPier）：盖梁、抗震挡块、支座垫石、墩柱、系梁、承台、桩基等参数化生成
  - T 梁（Tbeam）：梁长、梁高、翼缘、腹板、马蹄、横隔板等参数化生成
- **构件数据持久化**：构件以块（Block）形式写入 DWG，参数保存在块的扩展字典（TYPE / INFO Xrecord）中，重新打开图纸后可完整还原模型
- **项目管理器**：树形展示柱式墩 / T 梁 / 桥梁及其块实例，支持右键菜单操作，可从当前图纸自动扫描重建
- **属性编辑器**：可视化编辑选中构件的参数
- **日志面板**：运行日志实时输出
- **CAD 交互工具集**：图层 / 线型 / 标注样式初始化、多段线、实体拉伸、标注、Jig 拖拽、视图控制等封装

## 项目结构

```
CadBridgeDesigner/
├── CadBridgeDesigner.Core/     # 核心层：CAD 命令定义、事件管理、停靠面板（PaletteSet）宿主，供 Tool 实现接口注入
├── CadBridgeDesigner.Tool/     # 业务层：桥梁构件建模、WPF 视图（MVVM）、CAD 工具封装
├── CadBridgeDesigner.TestTool/ # 测试工具：LOAD_TEST_DLL 命令，从 out/ 目录热加载插件便于调试
└── out/                        # 编译输出目录（已加入 .gitignore）
```

- `CadBridgeDesigner.Core` 定义 `IExtensionApplication` 入口、`[CommandMethod]` 命令以及 `ICadCommandManagerHandler` / `ICadEventManagerHandler` 接口
- `CadBridgeDesigner.Tool` 在 AutoCAD 空闲时注入处理器实现，初始化三个 WPF 面板（项目管理器、属性编辑器、日志）并完成图层等配置

## 环境要求

- Windows x64
- AutoCAD 2024（使用其托管 DLL：`accoremgd.dll`、`acdbmgd.dll`、`acmgd.dll`）
- .NET Framework 4.8.1
- Visual Studio 2022（或 MSBuild 17+），C# 10

## 构建

1. **修改 AutoCAD DLL 引用路径**：三个 `.csproj` 中的 `HintPath` 指向本机 AutoCAD 2024 安装目录，请按实际路径修改，例如：

   ```xml
   <Reference Include="Acdbmgd">
     <HintPath>C:\Program Files\Autodesk\AutoCAD 2024\Acdbmgd.dll</HintPath>
     <Private>False</Private>
   </Reference>
   ```

2. 打开 `CadBridgeDesigner.sln`，选择 `Debug|x64` 或 `Release|x64` 编译；输出统一到 `out/` 目录。

   > `CadBridgeDesigner.Tool` 编译前会执行 `Properties\IncrementVersion.ps1` 自动递增版本号（设计时构建除外）。

## 在 AutoCAD 中使用

1. 启动 AutoCAD 2024，命令行执行 `NETLOAD`，选择 `out\CadBridgeDesigner.Tool.dll`（或通过 `out\CadBridgeDesigner.Core.dll` 加载命令入口）。
2. 插件加载后自动初始化停靠面板，并创建图层、线型、标注样式。
3. 可用命令：

   | 命令 | 说明 |
   |------|------|
   | `SHOW_VIEWS` | 显示项目管理器、属性编辑器、日志面板 |
   | `ADD_COLUMN_PIER` | 参数化添加柱式墩 |
   | `ADD_TBEAM` | 参数化添加 T 梁 |
   | `TEST_METHOD` | 测试命令 |
   | `LOAD_TEST_DLL` | （TestTool）从 `out/` 热加载 / 重新加载插件，便于开发调试 |

   > `LOAD_TEST_DLL` 默认插件目录写死为 `D:\WorkSpace\ProgramProject\C#\CadBridgeDesigner\out`，路径不同时请修改 `CadBridgeDesigner.TestTool\TestTool.cs` 中的 `PluginDirectory`。

## 开发说明

- UI 采用 MVVM 模式（`INotifyPropertyChanged` + `RelayCommand`），WPF 视图通过 `ElementHost` 嵌入 AutoCAD 的 `PaletteSet`
- 构件类型通过块扩展字典中的 `TYPE`（如 `COLUMN_PIER`、`TBEAM`）识别，`INFO` 保存全部参数
- 构件图形以二维轮廓转 `Region` 后拉伸为 `Solid3d` 实体生成
- 日志统一通过 `LogerTool.Log(...)` 输出到日志面板

## 许可证

本项目采用 [MIT License](https://opensource.org/licenses/MIT) 开源许可。

```
MIT License

Copyright (c) 2026 hkc

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
