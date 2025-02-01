[Root]
@RootInit:
VC_Init;
VC_Position: 0, -90000; 
VC_Confiner: 0, 100000, 950000, 550000;
# Zone
VC_DeadZone: 8, 10;
VC_SoftZone: 15, 15;
VC_Bias: 0, 5;
VC_Damping: 60000, 50000;
# Follow
VC_FollowPlayer;
VC_FollowOffset: 0;
# Regist Target
VC_MinFOV: 55000;
VC_MaxFOV: 100000;
VC_FOV: 55000;
return;