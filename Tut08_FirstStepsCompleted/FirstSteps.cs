using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private TransformComponent _cubeTransform, _cubeTransform1, _cubeTransform2;
        private ShaderEffectComponent cubeShader;

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to light green (intensities in R, G, B, A).
            RC.ClearColor = new float4(1, 1, 1, 1);
            //--------------------------------------------------------------------------------------------------------------------------
            // Create a scene with a cube
            // The three components: one XForm, one Material and the Mesh
            _cubeTransform = new TransformComponent {Scale = new float3(1, 1, 1), Translation = new float3(-2, -2, -2)};
            cubeShader = new ShaderEffectComponent
            { 
                Effect = SimpleMeshes.MakeShaderEffect(new float3 (0, 0, 1), new float3 (1, 1, 1),  4)
            };
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            // Assemble the cube node containing the three components
            var cubeNode = new SceneNodeContainer();
            cubeNode.Components = new List<SceneComponentContainer>();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(cubeShader);
            cubeNode.Components.Add(cubeMesh);
            //--------------------------------------------------------------------------------------------------------------------------
            // Create a scene with a cube
            // The three components: one XForm, one Material and the Mesh
            _cubeTransform1 = new TransformComponent {Scale = new float3(0.5f, 2, 0.5f), Translation = new float3(-20, 2, 2), Rotation= new float3(0,0,0)};
                
            
            var cubeShader1 = new ShaderEffectComponent{
            
                Effect = SimpleMeshes.MakeShaderEffect(new float3 (1, 0.9f, 1), new float3 (1, 1, 1),  4)
            };
            var cubeMesh1 = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            var cubeNode1 = new SceneNodeContainer();
            cubeNode1.Components = new List<SceneComponentContainer>();
            cubeNode1.Components.Add(_cubeTransform1);
            cubeNode1.Components.Add(cubeShader1);
            cubeNode1.Components.Add(cubeMesh1);
            //--------------------------------------------------------------------------------------------------------------------------
            // Create a scene with a cube
            // The three components: one XForm, one Material and the Mesh
            
           
           
            _cubeTransform2 = new TransformComponent {Scale = new float3(0.5f, 2, 0.5f), Translation = new float3(20, 2, 2), Rotation= new float3(1,2,3)};
                
            
            var cubeShader2 = new ShaderEffectComponent
            { 
                Effect = SimpleMeshes.MakeShaderEffect(new float3 (1, 0.9f, 1), new float3 (1, 1, 1),  4)
            };
           
            var cubeMesh2 = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            var wuerfel = new SceneNodeContainer();
            wuerfel.Components = new List<SceneComponentContainer>();
            wuerfel.Components.Add(_cubeTransform2);
            wuerfel.Components.Add(cubeShader2);
            wuerfel.Components.Add(cubeMesh2);

            
            
            //--------------------------------------------------------------------------------------------------------------------------
            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children = new List<SceneNodeContainer>();
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(cubeNode1);
            _scene.Children.Add(wuerfel);
            
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            Diagnostics.Log(TimeSinceStart);

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);//comment 123123 1231231231

            // Animate the camera angle
           // _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime ;
            var color =  new float3(M.Sin(0.3f*TimeSinceStart),M.Sin(0.2f*TimeSinceStart),M.Sin(0.9f*TimeSinceStart));

            // Animate the cube
            //Cube
            _cubeTransform.Rotation = new float3(0,5 * M.Sin(1 * TimeSinceStart),0);
            cubeShader.Effect.SetEffectParam("DiffuseColor",color);
            //cube1
            
            _cubeTransform1.Scale = new float3(M.Sin(1 * TimeSinceStart)+1,1,2);
            //wuerfel
            _cubeTransform2.Translation = new float3(20, M.Sin(1 * TimeSinceStart), 0);
            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);


            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}