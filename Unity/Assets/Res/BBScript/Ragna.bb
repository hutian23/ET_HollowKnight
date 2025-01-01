[Root]
@RootInit:
SetPos: 1100, 10000;
Gravity: 100000;
NumericType: MaxGravity, 150000;
NumericType: MaxFall, 45000;
NumericType: MaxJump, 2;
NumericType: MaxDash, 2;
NumericType: DashCount, 2;
NumericType: JumpCount, 2;
# 注册落地回调
RegistAirCheck;
# Input
RegistInput: RunHold;
RegistInput: SquatHold;
RegistInput: 2LPPressed;
RegistInput: 5LPPressed;
RegistInput: 5MPPressed;
RegistInput: DashPressed;
RegistInput: 5LPHold;
RegistInput: ShouRyuKen;
RegistInput: JumpPressed;
# Move
RegistMove: (Rg_Idle)
  MoveType: None;
  EndMove:
RegistMove: (Rg_Run)
  MoveType: Move;
  EndMove:
return;


[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
UpdateFlip;
#IdleAnim: Rg_IdleAnim;
InputBuffer: true;
DefaultWindow;
SetTransition: 'PreSquit';
SetMarker: 'Loop';
BBSprite: 'Idle_1', 4;
BBSprite: 'Idle_2', 4;
BBSprite: 'Idle_3', 4;
BBSprite: 'Idle_4', 4;
BBSprite: 'Idle_5', 5;
BBSprite: 'Idle_6', 5;
BBSprite: 'Idle_7', 6;
BBSprite: 'Idle_8', 4;
BBSprite: 'Idle_9', 4;
BBSprite: 'Idle_10', 4;
BBSprite: 'Idle_11', 4;
BBSprite: 'Idle_12', 4;
GotoMarker: 'Loop';
Exit;


[Rg_Run]
@Trigger:
InAir: false;
InputType: RunHold;
return;

@Main:
LogWarning: '222';
#PreRun
UpdateFlip;
#SetTransition: 'PreSquit';
InputBuffer: true;
DefaultWindow;
MoveX:13000;
BBSprite: 'PreRun_1', 1;
BBSprite: 'PreRun_2', 2;
#Run
BeginLoop: (InputType: RunHold)
  BBSprite: 'Run_1', 4;
  BBSprite: 'Run_2', 4;
  BBSprite: 'Run_3', 4;
  BBSprite: 'Run_4', 4;
  BBSprite: 'Run_5', 4;
  BBSprite: 'Run_6', 4;
  EndLoop:
CancelMoveX;
SetVelocityX: 0;
#Transition
#TransitionWindow;
BBSprite: 'RunToIdle_1', 3;
BBSprite: 'RunToIdle_2', 3;
BBSprite: 'RunToIdle_3', 3;
BBSprite: 'RunToIdle_4', 3;
Exit;