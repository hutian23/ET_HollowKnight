[Root]
@RootInit:
Thrones_Init;
Thrones_Goto: Thrones_Step1_Entry;
return;

# 一阶段
[Thrones_Step1_Entry]
@Main:
Thrones_Param: 1, PosX, 200;
Thrones_Param: 1, PosY, 11850;
Thrones_Param: 1, Flip, Right;
Thrones_SubState: 1, Boss_Gesture;
Thrones_Param: 2, PosX, 7200;
Thrones_Param: 2, PosY, 10300;
Thrones_Param: 2, Flip, Right;
Thrones_SubState: 2, Boss_Gesture;
Thrones_Param: 3, PosX, -7200;
Thrones_Param: 3, PosY, 10300;
Thrones_Param: 3, Flip, Left;
Thrones_SubState: 3, Boss_Gesture;
Thrones_WaitFrame: 100;
Thrones_SubState: 1, Boss_Entry;
Thrones_WaitFrame: 150;
# 测试
ThronesTest: A
  Thrones_Init;
  Thrones_SubState: 1, Boss_Dead;
  Thrones_WaitFrame: 60;
  Thrones_Param: 1, PosX, 200;
  Thrones_Param: 1, PosY, 11850;
  Thrones_Param: 1, Flip, Right;
  Thrones_SubState: 1, Boss_Wounded;
  EndThronesTest:
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 20)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:
BeginIf: (Random: ran < 40)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:

[Thrones_Step1_Dstab]
@Main:
Thrones_SubState: 1, Boss_Dstab;
Thrones_WaitFrame: 120;
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 20)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:
BeginIf: (Random: ran < 40)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:

[Thrones_Step1_Dash]
@Main:
Random: ran, 0, 1;
BeginIf: (Random: ran == 0)
  Thrones_Param: 1, Flip, Left;
  Thrones_Param: 1, PosX, -10500;
  Thrones_Param: 1, PosY, 3100;
  Thrones_Param: 1, VelX, -100000;
  Thrones_Param: 1, ReachX, 9500;
  Thrones_Param: 1, Direction, Right;
  EndIf:
BeginIf: (Random: ran == 1)
  Thrones_Param: 1, Flip, Right;
  Thrones_Param: 1, PosX, 10500;
  Thrones_Param: 1, PosY, 3100;
  Thrones_Param: 1, VelX, -100000;
  Thrones_Param: 1, ReachX, -9500;
  Thrones_Param: 1, Direction, Left;
  EndIf:
Thrones_SubState: 1, Boss_Dash;
Thrones_WaitFrame: 120;
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 20)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:
BeginIf: (Random: ran < 75)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:

[Thrones_Step1_WallThrow]
@Main:
Random: ran, 0, 1;
BeginIf: (Random: ran == 0)
  Thrones_Param: 1, Flip, Left;
  Thrones_Param: 1, PosX, 14600;
  Thrones_Param: 1, PosY, 5000;
  EndIf:
BeginIf: (Random: ran == 1)
  Thrones_Param: 1, Flip, Right;
  Thrones_Param: 1, PosX, -14600;
  Thrones_Param: 1, PosY, 5000;
  EndIf:
Thrones_SubState: 1, Boss_WallThrow;
Thrones_WaitFrame: 120;
# Exit
Random: ran, 0, 100;
BeginIf: (Random: ran < 10)
  Thrones_Goto: Thrones_Step1_WallThrow;
  EndIf:
BeginIf: (Random: ran < 70)
  Thrones_Goto: Thrones_Step1_Dstab;
  EndIf:
BeginIf: (Random: ran <= 100)
  Thrones_Goto: Thrones_Step1_Dash;
  EndIf:

# 二阶段
