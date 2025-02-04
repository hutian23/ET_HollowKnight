[Root]
@RootInit:
VC_RootInit;
VC_Camera: DefaultCamera;
return;

# 当前场景的默认摄像机
[DefaultCamera]
@Main:
VC_Init;
VC_Sensor: 0, 0, 100000, 10000;
VC_Confiner: 0, 100000, 950000, 950000;
# Zone
VC_DeadZone: 8, 5;
VC_SoftZone: 15, 15;
VC_Bias: 0, 10;
VC_Damping: 50000, 50000;
# Follow
VC_FollowPlayer;
VC_FollowOffset: 0;
# FOV
VC_MinFOV: 20000;
VC_MaxFOV: 150000;
VC_FOV: 90000;
return;

# 战斗时摄像头
[CombatCamera]
@Main:
VC_Init;
VC_Confiner: 0, 100000, 450000, 950000;
# Zone
VC_DeadZone: 8, 5;
VC_SoftZone: 13, 15;
VC_Bias: 0, 10;
VC_Damping: 60000, 50000;
# Follow
VC_FollowPlayer;
VC_FollowOffset: 0;
# FOV
VC_MinFOV: 20000;
VC_MaxFOV: 150000;
VC_FOV: 105000;
# VC_FOVTransition: 60000, 8000;
return;

[Trigger_1]
@TriggerEnter:
# 摄像头中心点移动到该位置
# VC_MoveTo: 0, 10000, 5000;
# WaitFrame: 100;
# VC_MoveToPlayer;
# VC_Camera_Goto: DefaultCamera;
LogWarning: 'Hello World';
return;

@TriggerExit:
return;