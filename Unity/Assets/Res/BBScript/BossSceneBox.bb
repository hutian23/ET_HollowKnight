[Root]
@RootInit:
SceneBoxInit;
return;

[A]
@TriggerStay:
LogWarning: 'Hello_World';
return;

[Plateform]
@TriggerStay:
LogWarning: 'Land';
return;

@TriggerExit:
LogWarning: 'Exit';
return;