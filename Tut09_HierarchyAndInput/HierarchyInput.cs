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
    public class HierarchyInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private float _camSpeed = 0;
        private TransformComponent _baseTransform;
        private TransformComponent _bodyTransform;
        private TransformComponent _upperArmTransform;
        private TransformComponent _lowerArmTransform;
        private TransformComponent _leftFingerTransform;
        private TransformComponent _rightFingerTransform;

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 6, 0)
            };

            _upperArmTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 4, 0)
            };

            _lowerArmTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-2, 4, 0)
            };            

            _leftFingerTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-0.5f, 4.5f, 0)
            };     

            _rightFingerTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0.5f, 4.5f, 0)
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
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(0.7f, 0.7f, 0.7f), 5)
                            },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },
                    // RED BODY
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _bodyTransform,

                            // SHADER EFFECT COMPONENT
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(1.0f, 0.2f, 0.2f), new float3(0.7f, 0.7f, 0.7f), 5)
                            },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                        },
                        Children = new List<SceneNodeContainer>
                        {
                            // PIVOT GREEN UPPER ARM
                            new SceneNodeContainer
                            {
                                Components = new List<SceneComponentContainer>
                                {
                                    // TRANSFROM COMPONENT
                                    _upperArmTransform
                                },
                                Children = new List<SceneNodeContainer>
                                {
                                    // GREEN UPPER ARM
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            // TRANSFROM COMPONENT
                                            new TransformComponent
                                            {
                                                Rotation = new float3(0, 0, 0),
                                                Scale = new float3(1, 1, 1),
                                                Translation = new float3(0, 4, 0)
                                            },

                                            // SHADER EFFECT COMPONENT
                                            new ShaderEffectComponent
                                            {
                                                Effect = SimpleMeshes.MakeShaderEffect(new float3( 0.2f, 1.0f, 0.2f), new float3(0.7f, 0.7f, 0.7f), 5)
                                            },

                                            // MESH COMPONENT
                                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                        },
                                        Children = new List<SceneNodeContainer>
                                        {
                                            // PIVOT BLUE LOWER ARM
                                            new SceneNodeContainer
                                            {
                                                Components = new List<SceneComponentContainer>
                                                {
                                                    // TRANSFROM COMPONENT
                                                    _lowerArmTransform
                                                },
                                                Children = new List<SceneNodeContainer>
                                                {
                                                    // BLUE LOWER ARM
                                                    new SceneNodeContainer
                                                    {
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            // TRANSFROM COMPONENT
                                                            new TransformComponent
                                                            {
                                                                Rotation = new float3(0, 0, 0),
                                                                Scale = new float3(1, 1, 1),
                                                                Translation = new float3(0, 4, 0)
                                                            },

                                                            // SHADER EFFECT COMPONENT
                                                            new ShaderEffectComponent
                                                            {
                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3( 0.2f, 0.2f, 1.0f), new float3(0.7f, 0.7f, 0.7f), 5)
                                                            },

                                                            // MESH COMPONENT
                                                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                                        },
                                                        Children = new List<SceneNodeContainer>
                                                        {
                                                            // PIVOT LEFT FINGER
                                                            new SceneNodeContainer
                                                            {
                                                                Components = new List<SceneComponentContainer>
                                                                {
                                                                    // TRANSFROM COMPONENT
                                                                    _leftFingerTransform
                                                                },
                                                                Children = new List<SceneNodeContainer>
                                                                {
                                                                    // LEFT FINGER
                                                                    new SceneNodeContainer
                                                                    {
                                                                        Components = new List<SceneComponentContainer>
                                                                        {
                                                                            // TRANSFROM COMPONENT
                                                                            new TransformComponent
                                                                            {
                                                                                Rotation = new float3(0, 0, 0),
                                                                                Scale = new float3(1, 1, 1),
                                                                                Translation = new float3(0, 2.5f, 0)
                                                                            },

                                                                            // SHADER EFFECT COMPONENT
                                                                            new ShaderEffectComponent
                                                                            {
                                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3( 0.4f, 0.4f, 0.4f), new float3(0.7f, 0.7f, 0.7f), 5)
                                                                            },

                                                                            // MESH COMPONENT
                                                                            SimpleMeshes.CreateCuboid(new float3(1, 5, 1))
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            // PIVOT RIGHT FINGER
                                                            new SceneNodeContainer
                                                            {
                                                                Components = new List<SceneComponentContainer>
                                                                {
                                                                    // TRANSFROM COMPONENT
                                                                    _rightFingerTransform
                                                                },
                                                                Children = new List<SceneNodeContainer>
                                                                {
                                                                    // RIGHT FINGER
                                                                    new SceneNodeContainer
                                                                    {
                                                                        Components = new List<SceneComponentContainer>
                                                                        {
                                                                            // TRANSFROM COMPONENT
                                                                            new TransformComponent
                                                                            {
                                                                                Rotation = new float3(0, 0, 0),
                                                                                Scale = new float3(1, 1, 1),
                                                                                Translation = new float3(0, 2.5f, 0)
                                                                            },

                                                                            // SHADER EFFECT COMPONENT
                                                                            new ShaderEffectComponent
                                                                            {
                                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3( 0.4f, 0.4f, 0.4f), new float3(0.7f, 0.7f, 0.7f), 5)
                                                                            },

                                                                            // MESH COMPONENT
                                                                            SimpleMeshes.CreateCuboid(new float3(1, 5, 1))
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

            // Inputs
            float body_rotation_y = _bodyTransform.Rotation.y;  
            body_rotation_y += 2f * Keyboard.LeftRightAxis * Time.DeltaTime;
            _bodyTransform.Rotation = new float3(0, body_rotation_y, 0);
            
            float upper_arm_rotation_x = _upperArmTransform.Rotation.x;  
            upper_arm_rotation_x += 2f * Keyboard.UpDownAxis * Time.DeltaTime;
            _upperArmTransform.Rotation = new float3(upper_arm_rotation_x, 0, 0);
            
            float lower_arm_rotation_x = _lowerArmTransform.Rotation.x;  
            lower_arm_rotation_x += 2f * Keyboard.WSAxis * Time.DeltaTime;
            _lowerArmTransform.Rotation = new float3(lower_arm_rotation_x, 0, 0);

            if( Mouse.LeftButton){
                _camSpeed = Mouse.Velocity.x;
            }
            else {
                _camSpeed = _camSpeed * 0.9f;
            }

            _camAngle = _camAngle + 0.005f * _camSpeed * Time.DeltaTime;
            
            float left_finger_rotation_z = _leftFingerTransform.Rotation.z;  
            left_finger_rotation_z += 2f * Keyboard.ADAxis * Time.DeltaTime;
            if( left_finger_rotation_z > 1.5f) {
                left_finger_rotation_z = 1.5f;
            }
            if( left_finger_rotation_z < 0) {
                left_finger_rotation_z = 0;
            }
            _leftFingerTransform.Rotation = new float3(0, 0, left_finger_rotation_z);
            
            float right_finger_rotation_z = _rightFingerTransform.Rotation.z;  
            right_finger_rotation_z -= 2f * Keyboard.ADAxis * Time.DeltaTime;
            if( right_finger_rotation_z < -1.5f) {
                right_finger_rotation_z = -1.5f;
            }
            if( right_finger_rotation_z > 0) {
                right_finger_rotation_z = 0;
            }
            _rightFingerTransform.Rotation = new float3(0, 0, right_finger_rotation_z);



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