// namespace ET.Client
// {
//     [FriendOf(typeof(InputWait))]
//     public class Classic_ShouRyuKen_InputHandler : BBInputHandler
//     {
//         public override string GetHandlerType()
//         {
//             return "Classic_ShouRyuKen";
//         }
//
//         public override string GetBufferType()
//         {
//             return "ShouRyuKen";
//         }
//
//         public override async ETTask<InputBuffer> Handle(InputWait self, ETCancellationToken token)
//         {
//             TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
//             b2Body body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
//
//             //1. Right: 6; Left: 4
//             WaitInput wait1 = await self.Wait(OP: BBOperaType.RIGHT | BBOperaType.LEFT | BBOperaType.UPRIGHT | BBOperaType.UPLEFT,
//                 waitType: FuzzyInputType.OR,
//                 () =>
//                 {
//                     //判断朝向
//                     FlipState curFlip = (FlipState)body.GetFlip();
//
//                     switch (curFlip)
//                     {
//                         case FlipState.Right:
//                             return (self.Ops & BBOperaType.RIGHT) != 0 || (self.Ops & BBOperaType.UPRIGHT) != 0;
//                         case FlipState.Left:
//                             return (self.Ops & BBOperaType.LEFT) != 0 || (self.Ops & BBOperaType.UPLEFT) != 0;
//                         default:
//                             return false;
//                     }
//                 });
//             if(wait1.Error != WaitTypeError.Success) return InputBuffer.None;
//             Log.Warning("Step1");
//             
//             //2. Right: 2 3; Left: 1 2
//             WaitInput wait2 = await self.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT | BBOperaType.DOWNRIGHT,
//                 waitType: FuzzyInputType.OR, () =>
//                 {
//                     FlipState curFlip = (FlipState)body.GetFlip();
//
//                     switch (curFlip)
//                     {
//                         case FlipState.Right:
//                             return (self.Ops & BBOperaType.DOWN) != 0 || (self.Ops & BBOperaType.DOWNRIGHT) != 0;
//                         case FlipState.Left:
//                             return (self.Ops & BBOperaType.DOWN) != 0 || (self.Ops & BBOperaType.DOWNLEFT) != 0;
//                         default:
//                             return false;
//                     }
//                 }, 4);
//             if(wait2.Error != WaitTypeError.Success) return InputBuffer.None;
//             Log.Warning("Step2");
//             
//             //3. Right: 6; Left: 4
//             WaitInput wait3 = await self.Wait(OP: BBOperaType.RIGHT | BBOperaType.LEFT,
//                 waitType: FuzzyInputType.OR,
//                 () =>
//                 {
//                     //判断朝向
//                     FlipState curFlip = (FlipState)body.GetFlip();
//
//                     switch (curFlip)
//                     {
//                         case FlipState.Right:
//                             return (self.Ops & BBOperaType.RIGHT) != 0 || (self.Ops & BBOperaType.UPRIGHT) != 0;
//                         case FlipState.Left:
//                             return (self.Ops & BBOperaType.LEFT) != 0 || (self.Ops & BBOperaType.UPLEFT) != 0;
//                         default:
//                             return false;
//                     }
//                 },4);
//             if(wait3.Error != WaitTypeError.Success) return InputBuffer.None;
//             Log.Warning("Step3");
//             
//             if ((wait3.OP & BBOperaType.Y) != 0 && self.WasPressedThisFrame(BBOperaType.Y))
//             {
//                 Log.Warning("Success");
//                 return self.CreateBuffer(15); }
//
//             WaitInput wait4 = await self.Wait(OP: BBOperaType.Y, waitType: FuzzyInputType.OR, () =>
//             {
//                 bool wasPressedThisFrame = self.WasPressedThisFrame(BBOperaType.Y);
//                 return wasPressedThisFrame;
//             },4);
//             if (wait4.Error != WaitTypeError.Success) return InputBuffer.None;
//             Log.Warning("Step4");
//             
//             return self.CreateBuffer(15);
//         }
//     }
// }