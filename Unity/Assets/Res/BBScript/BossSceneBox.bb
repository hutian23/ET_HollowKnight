[Root]
@RootInit:
SceneBoxInit;
# 摄像机相关触发器
VC_Trigger: CameraTrigger_1, 150000, -100000, 120000, 100000;
return;

[A]
@TriggerStay:
return;

[Plateform]
@TriggerStay:
return;

@TriggerExit:
return;

# 摄像机事件
[CameraTrigger_1]
@TriggerEnter:
# VC_FOVTransition: 60000, 25000;
VC_Camera: CombatCamera;
return;

@TriggerExit:
VC_Camera: DefaultCamera;
return;