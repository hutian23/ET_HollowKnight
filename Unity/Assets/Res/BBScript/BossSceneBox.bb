[Root]
@RootInit:
SceneBoxInit;
# 摄像机相关触发器
VC_Trigger: CameraTrigger_1, 300000, -100000, 300000, 100000;
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
VC_FOVTransition: 110000, 35000;
return;

@TriggerExit:
VC_FOVTransition: 90000, 20000;
return;