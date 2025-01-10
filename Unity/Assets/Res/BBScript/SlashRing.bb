[Root]
@RootInit:
RegistMove: (SlashRing_Idle)
  MoveType: None;
  EndMove:
return;

[SlashRing_Idle]
@Trigger:
return;

@Main:
# 从哪个组件上查询值
SetVelocityX: {Timeline.VelX};
SetVelocityY: {Timeline.VelY};
AccelerationX: {Timeline.Accel}, {Timeline.MaxV};
BBSprite: 'Slash_1', 3;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
Dispose;