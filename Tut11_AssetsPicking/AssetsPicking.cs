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
    public class AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;                private float _camAngle = 0;
        private SceneRenderer _sceneRenderer;
        private TransformComponent _baseTransform;
        private TransformComponent _zugTransform;
        private TransformComponent _vorderRad_L_Transform;
                private TransformComponent _vorderRad_L_Transform1;
        private TransformComponent _vorderRad_R_Transform1;
        private TransformComponent _vorderRad_R_Transform;
        private TransformComponent _hinterRad_L_Transform;
        private TransformComponent _hinterRad_R_Transform;
        private ShaderEffectComponent _zugShader;
        private TransformComponent _carTrans;

        private ScenePicker _scenePicker;
          private PickResult _currentPick;
  private float3 _oldColor;


        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(1, 1, 1), 5)
                            },

                            // MESH COMPONENT
                            // SimpleAssetsPickinges.CreateCuboid(new float3(10, 10, 10))
                            SimpleMeshes.CreateCuboid(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);
            
           

            _scene = AssetStorage.Get<SceneContainer>("rad.fus");
             _scenePicker = new ScenePicker(_scene);
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
           // _baseTransform.Rotation = new float3(0, M.MinAngle(TimeSinceStart), 0);
           // _carTrans = _scene.Children.FindNodes(node => node.Name == "Plane")?.FirstOrDefault()?.GetTransform.Translation();
            _zugTransform = _scene.Children.FindNodes(node => node.Name == "Cube")?.FirstOrDefault()?.GetTransform();
            _zugShader = _scene.Children.FindNodes(node => node.Name == "Cube")?.FirstOrDefault()?.GetComponent<ShaderEffectComponent>();
            _vorderRad_L_Transform = _scene.Children.FindNodes(node => node.Name == "Rad_L_vorne.001")?.FirstOrDefault()?.GetTransform();
            _vorderRad_R_Transform = _scene.Children.FindNodes(node => node.Name == "Rad_R_vorne.001")?.FirstOrDefault()?.GetTransform();
            _vorderRad_L_Transform1 = _scene.Children.FindNodes(node => node.Name == "Rad_L_vorne.001")?.FirstOrDefault()?.GetTransform();
            _vorderRad_R_Transform1 = _scene.Children.FindNodes(node => node.Name == "Rad_R_vorne.001")?.FirstOrDefault()?.GetTransform();
            _hinterRad_L_Transform = _scene.Children.FindNodes(node => node.Name == "Rad_L_vorne")?.FirstOrDefault()?.GetTransform();
            _hinterRad_R_Transform = _scene.Children.FindNodes(node => node.Name == "Rad_R_vorne")?.FirstOrDefault()?.GetTransform();
            //_zugShader.Effect.SetEffectParam("DiffuseColor", new float3(1,1,1));

            float _lenken_vorne = _vorderRad_L_Transform1.Rotation.x;//Links Rechts Lenkung Vorne
            float _gas = _hinterRad_L_Transform.Rotation.y;//Vor und zurück fahren
            _lenken_vorne += 0.5f * Keyboard.ADAxis;

            _gas += -3f * Keyboard.WSAxis;
            _vorderRad_L_Transform.Rotation = new float3(0,_gas,_lenken_vorne);
            _vorderRad_R_Transform.Rotation = new float3(0,_gas,_lenken_vorne);
            _hinterRad_L_Transform.Rotation = new float3(0,_gas,0);
            _hinterRad_R_Transform.Rotation = new float3(0,_gas,0);

           float _zugUpDown = _zugTransform.Rotation.x;
           _zugUpDown += 0.5f * Keyboard.UpDownAxis;
           _zugTransform.Rotation = new float3(0,0,_zugUpDown);

           /*--------------------------------------------------------------------
             float newYRot = _carTrans.Rotation.y ;
            _carTrans.Rotation = new float3(0, newYRot, 0);
              float3 newPos = _carTrans.Position;
            newPos.x += posVel * M.Sin(newYRot);
            newPos.z += posVel * M.Cos(newYRot);
            _carTrans.Position = newPos;
           //--------------------------------------------------------------------*/
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            _camAngle = _camAngle + 33.3333f * M.Pi/180.0f * DeltaTime ;
            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 10) * float4x4.CreateRotationY(_camAngle);
            if (Mouse.LeftButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
                  PickResult newPick = null;
                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    newPick = pickResults[0];
                }
                if (newPick?.Node != _currentPick?.Node)
                {
                   if (_currentPick != null)
                    {
                        ShaderEffectComponent shaderEffectComponent = _currentPick.Node.GetComponent<ShaderEffectComponent>();
                        shaderEffectComponent.Effect.SetEffectParam("DiffuseColor", _oldColor);
                    }
                    if (newPick != null)
                    {
                        ShaderEffectComponent shaderEffectComponent = newPick.Node.GetComponent<ShaderEffectComponent>();
                        _oldColor = (float3)shaderEffectComponent.Effect.GetEffectParam("DiffuseColor");
                        shaderEffectComponent.Effect.SetEffectParam("DiffuseColor", new float3(1, 0.4f, 0.4f));
                        
                    }
                    _currentPick = newPick;
                }
            }



            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45� Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
