# STG(ShootingGame)_SpaceCombat总结

# 总览

<img title="" src="./Previews/STG_SpaceCombat.png" alt="project_summary" width="754">

![view_1](./Previews/view_1.png)

![view_2](./Previews/view_2.png)

![view_3](./Previews/view_3.png)

![project_structure](./Previews/Project_Structure.png)


# 3C

### Character

- Player
- Enemies

### Control

- 使用Input System适配多种输入
  
  - 键鼠
  - 手柄（摇杆）

### Camera

- 分了专门渲染UI的Camera和主要渲染游戏内容的Camera

- 做了渲染的后处理

---

# Stream（流）

### 背景滚动（创建卷轴）

视差背景的基础也是基于改变background的position值，

只是近处的改变的快，远处的改变的相对较慢。

更简单的创建卷轴方式：改变repeat的材质的offset来实现滚动。

### 玩家输入

使用Input System适配手柄和键鼠。

### 优化手感（为移动加减速）

### 射弹以及对象池

### AI管理

### 生命值

1. 要么通过已经挂载的脚本继承实现该系统。（√）

2. 要么通过为每个具生命值的物体添加脚本。

### 物理碰撞

### UI

- HUD（相机范围）
- 血条（全局范围）
- 通过一个专门的摄像机渲染

### Energy System

- 暴走
- 闪避

### 画面

- Post Processing后处理
- bloom发光

### Enemy Mananger

- 敌人波次
- 敌人随波次不断增强

### UI动画

- 代码？花样少，实现较繁琐，性能好（√）

- 动画器？花样多，实现简单，性能差

### 音频

- AudioManager

### 异步场景加载

- SceneLoader

### 得分系统

### 子弹时间

- TimeController-控制timescale缓动（lerp以及协程）

### 暂停菜单

### 游戏状态

### 玩家道具——导弹

- 范围伤害实现：

- 1、添加一个trigger，通过ontriggerEnter2D来检查
  
  2、通过计算距离检查，通过自定义函数进行进行检测
  
  3、通过OverlapCircleAll物理重叠检测函数（√）

### 粒子系统（特效）

### 完成一个游戏循环

- 指的是开始游戏-游戏结束-重开游戏这样的循环。

### 数据持久化（保存高分）

- 通过json文件保存到本地

- 读取json文件显示高分榜

### 设计boss战

- ai

### 玩家受伤无敌时间

- 协程以及破环碰撞发生条件（isTrigger=true）

### 玩家道具——战利品

- 使用从预制体继承的预制体来制作不同的战利品

### 使用Profile检查项目性能

### 打包导出项目

---

# System

### 视差背景系统

### 管理玩家输入

### 射弹系统

### 对象池

### 敌人管理器

### 生命值系统

### 物理系统

### 音频管理

### 场景加载

### UI

### 时间管理

### 得分系统和高分系统（持久化）

### 掉落系统

- 战利品
- 允许设置掉落率半分比

### 独特的系统

- 能量系统（支持玩家发动技能）
  
  - 暴走
  - 闪避

- 主动道具
  
  - 导弹

### boss战

---

# Resources

学习参考refference：

[[Unity] 横版卷轴射击游戏 制作教程 Ep.01 项目创建 | 简易背景卷动 | URP模板 | 独立游戏 | 游戏开发_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1rU4y187Wf/?spm_id_from=333.999.0.0&vd_source=56e8fdea1840126840d1260a558908b9)

请关注follow：

[阿严Gaming的个人空间_哔哩哔哩_bilibili](https://space.bilibili.com/27164588)
