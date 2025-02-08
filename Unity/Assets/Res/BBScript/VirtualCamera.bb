[Root]
@RootInit:
CM_Init;
CM_Camera: DefaultCamera, 2DCamera;
return;


[DefaultCamera]
@RootInit:
CM_Priority: 100;
return;