﻿using System.Runtime.Serialization;

namespace Testbed.Abstractions
{
    [DataContract]
    public class TestSettings
    {
        public TestSettings()
        {
            Reset();
        }

        public void Reset()
        {
            TestIndex = 0;
            WindowWidth = 1600;
            WindowHeight = 900;
            this.TimeScale = 60.0f;
            VelocityIterations = 8;
            PositionIterations = 3;
            DrawShapes = true;
            DrawJoints = true;
            DrawAABBs = false;
            DrawContactPoints = false;
            DrawContactNormals = false;
            DrawContactImpulse = false;
            DrawFrictionImpulse = false;
            DrawCOMs = false;
            DrawStats = false;
            DrawProfile = false;
            EnableWarmStarting = true;
            EnableContinuous = true;
            EnableSubStepping = false;
            EnableSleep = true;
            Pause = false;
            SingleStep = false;
            DrawStats = false;
            DrawProfile = false;
        }

        [DataMember]
        public int TestIndex;

        [DataMember]
        public int ETItemIndex;

        [DataMember]
        public bool ShowHitBox;

        [DataMember]
        public bool ShowHurtBox;

        [DataMember]
        public bool ShowThrowBox;

        [DataMember]
        public bool ShowSquashBox;

        [DataMember]
        public bool ShowProximityBox;

        [DataMember]
        public bool ShowOtherBox;

        [DataMember]
        public bool ShowGizmos;
        
        [DataMember]
        public int WindowWidth;

        [DataMember]
        public int WindowHeight;

        [DataMember]
        public float TimeScale;

        [DataMember]
        public int VelocityIterations;

        [DataMember]
        public int PositionIterations;

        [DataMember]
        public bool DrawShapes;

        [DataMember]
        public bool DrawJoints;

        [DataMember]
        public bool DrawAABBs;

        [DataMember]
        public bool DrawContactPoints;

        [DataMember]
        public bool DrawContactNormals;

        [DataMember]
        public bool DrawContactImpulse;

        [DataMember]
        public bool DrawFrictionImpulse;

        [DataMember]
        public bool DrawGizmos;

        [DataMember]
        public bool DrawCOMs;

        [DataMember]
        public bool DrawStats;

        [DataMember]
        public bool DrawProfile;

        [DataMember]
        public bool EnableWarmStarting;

        [DataMember]
        public bool EnableContinuous;

        [DataMember]
        public bool EnableSubStepping;

        [DataMember]
        public bool EnableSleep;
        
        public bool Pause;

        public bool SingleStep;
        
        public long instanceId;

        public long StepCount;
    }
}