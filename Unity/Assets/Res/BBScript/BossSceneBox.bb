[Root]
@RootInit:
SceneBoxInit;
# 摄像机相关触发器
VC_Trigger: Trigger_1, 0, -50000, 120000, 10000;
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
[Trigger_1]
@TriggerEnter:
LogWarning: 'Hello World';
return;