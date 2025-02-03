[Root]
@RootInit:
VC_Init;
VC_Position: 0, -90000; 
VC_Confiner: 0, 100000, 950000, 550000;
# Zone
VC_DeadZone: 0, 6;
VC_SoftZone: 2, 9;
VC_Bias: 0, 3;
VC_Damping: 30000, 50000;
# Follow
VC_FollowPlayer;
VC_FollowOffset: 10;
# FollowTransition: 随玩家速度方向改变调整镜头偏移的方向
VC_FollowTransition: true;
VC_FollowTransition_MinVel: 5100;
VC_FollowTransition_Accel: 1800;
# VC_FollowTransition_MaxVel: 2500;
# Regist Target
VC_MinFOV: 85000;
VC_MaxFOV: 100000;
VC_FOV: 85000;
return;