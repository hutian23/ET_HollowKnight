[Root]
@RootInit:
VC_Init;
VC_Position: 0, -90000; 
VC_Confiner: 0, 100000, 950000, 950000;
# Zone
VC_DeadZone: 8, 5;
VC_SoftZone: 15, 15;
VC_Bias: 0, 10;
VC_Damping: 50000, 50000;
# Follow
VC_FollowPlayer;
VC_FollowOffset: 0;
# FollowTransition: 随玩家速度方向改变调整镜头偏移的方向
# VC_FollowTransition: true;
# VC_FollowTransition_MinVel: 5100;
# VC_FollowTransition_Accel: 1800;
# VC_FollowTransition_MaxVel: 2500;
# FOV
VC_MinFOV: 20000;
VC_MaxFOV: 150000;
VC_FOV: 90000;
return;