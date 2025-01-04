[Boss]
@TriggerEnter:
LogWarning: 'Hello World';
return;

@TriggerStay:
LogWarning: 'HelloWorld';
return;

[Plateform]
@TriggerEnter:
return;

@TriggerStay:
return;

@CollisionStay:
BeginIf: (CollisionType: Player)
LogWarning: 'collide with player';
EndIf:
return;