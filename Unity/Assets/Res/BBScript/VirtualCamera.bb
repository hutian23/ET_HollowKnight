[Root]
@RootInit:
VC_RootInit;
VC_Camera: DefaultCamera;
return;

# 当前场景的默认摄像机
[DefaultCamera]
@Main:
VC_Init;
# TargetGroup
# VCExtension_TargetGroup;
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
VC_FOV: 70000;
return;

# 战斗时摄像头
[CombatCamera]
@Main:
VC_MoveTo: 150000, -100000, 20000;
return;