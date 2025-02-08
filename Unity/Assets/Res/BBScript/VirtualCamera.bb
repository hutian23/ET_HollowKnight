[Root]
@RootInit:
CM_Init;
# DefaultCamera
CM_2DCamera: DefaultCamera;
CM_Priority: DefaultCamera, 100;
CM_OrthoSize: DefaultCamera, 105000;
# Follow
CM_FollowPlayer;
return;


[DefaultCamera]
@RootInit:
CM_Priority: DefaultCamera, 100;
return;