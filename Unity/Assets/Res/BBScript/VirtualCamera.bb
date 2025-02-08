[Root]
@RootInit:
CM_Init;
# DefaultCamera
CM_2DCamera: DefaultCamera;
CM_Priority: DefaultCamera, 100;
CM_OrthoSize: DefaultCamera, 55000;
CM_DeadZone: DefaultCamera, 30, 30;
CM_SoftZone: DefaultCamera, 50, 50;
CM_Bias: DefaultCamera, 0, 50;
# Follow
CM_FollowPlayer;
return;


[DefaultCamera]
@RootInit:
CM_Priority: DefaultCamera, 100;
return;