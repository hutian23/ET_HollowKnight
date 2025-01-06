[Root]
@RootInit:
PoolObject: 'SlashRing', 4;
SetPos: 3000, 5000;
# RegistMove: (Boss_Dash)
#   MoveType: None;
#   EndMove:
# RegistMove: (Boss_Dstab)
#   MoveType: None;
#   EndMove:
RegistMove: (Boss_WallThrow)
  MoveType: None;
  EndMove:
return;

[Boss_Dash]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetPos: -10500, 3100;
BBSprite: 'Arrive_1', 3;
BBSprite: 'Arrive_2', 3;
BBSprite: 'Arrive_3', 3;
BBSprite: 'Arrive_4', 3;
BBSprite: 'Anticipate_1', 5;
BBSprite: 'Anticipate_2', 5;
BBSprite: 'Anticipate_3', 5;
BBSprite: 'Anticipate_4', 5;
BBSprite: 'Anticipate_5', 5;
BBSprite: 'Anticipate_6', 5;
SetVelocityX: -100000;
BBSprite: 'Dash_1', 3;
BBSprite: 'Dash_2', 3;
BeginLoop: (ReachX: 9500, Right)
  BBSprite: 'Dash_3', 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Recover_1', 3;
BBSprite: 'Recover_2', 3;
BBSprite: 'Recover_3', 3;
BBSprite: 'Recover_4', 3;
BBSprite: 'Recover_5', 3;
BBSprite: 'Recover_6', 3;
BBSprite: 'Recover_7', 3;
BBSprite: 'Recover_8', 3;
BBSprite: 'Recover_9', 3;
BBSprite: 'Recover_10', 3;
BBSprite: 'Leave_1', 3;
BBSprite: 'Leave_2', 5;
Exit;

[Boss_Dstab]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
Test_SetPlayerPosY: 13000;
BBSprite: 'Arrive_1', 4;
BBSprite: 'Arrive_2', 4;
BBSprite: 'Arrive_3', 4;
BBSprite: 'Arrive_4', 4;
BBSprite: 'Anticipate_1', 3;
BBSprite: 'Anticipate_2', 3;
BBSprite: 'Anticipate_3', 3;
BBSprite: 'Anticipate_4', 3;
BBSprite: 'Anticipate_5', 3;
BBSprite: 'Anticipate_6', 3;
BBSprite: 'Anticipate_7', 3;
SetVelocityY: -130000;
BBSprite: 'Dstab_1', 2;
BBSprite: 'Dstab_2', 6;
SetVelocityY: 0;
BBSprite: 'Land_1', 4;
BBSprite: 'Land_2', 4;
BBSprite: 'Land_3', 4;
BBSprite: 'Land_4', 6;
BBSprite: 'Land_5', 6;
BBSprite: 'Land_6', 6;
BBSprite: 'Leave_1', 4;
BBSprite: 'Leave_2', 4;
Exit;

[Boss_WallThrow]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
BBSprite: 'Arrive_1', 3;
BBSprite: 'Arrive_2', 3;
BBSprite: 'Arrive_3', 3;
BBSprite: 'Arrive_4', 3;
BBSprite: 'Anticipate_1', 4;
BBSprite: 'Anticipate_2', 4;
BBSprite: 'Anticipate_3', 4;
BBSprite: 'Anticipate_4', 4;
BBSprite: 'Anticipate_5', 4;
BBSprite: 'Anticipate_6', 4;
BBSprite: 'Anticipate_7', 4;
BBSprite: 'Anticipate_8', 4;
BBSprite: 'Throw_1', 1;
CreateBall: SlashRing
  BallParam: PosX, 10;
  EndCreateBall:
BBSprite: 'Throw_1', 7;
BBSprite: 'Throw_2', 4;
BBSprite: 'Throw_3', 4;
BBSprite: 'Recover_1', 4;
BBSprite: 'Recover_2', 4;
BBSprite: 'Leave_1', 4;
BBSprite: 'Leave_2', 4;
BBSprite: 'Leave_3', 4;
Exit;