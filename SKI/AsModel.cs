using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUIPckManager.OpenAs
{
    public partial class AsModel : Form
    {
        private Microsoft.DirectX.Direct3D.Device device;
        private float angle = 10f;
        private Mesh[] pwModel;
        private Microsoft.DirectX.DirectInput.Device keyb;
        private Microsoft.DirectX.DirectInput.Device mouse;
        private float ud;
        private float lr;
        private float rud;
        private float rlr;
        private Material pwMaterial;
        private Texture[] pwTexture;
        SkiReader ski;

        public AsModel(byte[] model)
        {
            InitializeComponent();
            SetStyle(ControlStyles.Opaque | ControlStyles.ResizeRedraw, true);
            MouseWheel += AsModel_MouseWheel;
            InitializeDevice();
            InitializeKeyboard();
            CameraPositioning();
            ski = new SkiReader(model);
            MakePWModel();
            Text = string.Format("Model Viewer, SKI Version: {0}", ski.SkiType);
        }

        private void AsModel_MouseWheel(object sender, MouseEventArgs e)
        {
            angle -= e.Delta / 150f;
        }

        public void InitializeDevice()
        {
            device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, new PresentParameters[]
            {
                new PresentParameters
                {
                    Windowed = true,
                    SwapEffect = SwapEffect.Discard,
                    AutoDepthStencilFormat = DepthFormat.D16,
                    EnableAutoDepthStencil = true,
                }
            });
            device.DeviceReset += new EventHandler(HandleResetEvent);
            device.RenderState.FillMode = FillMode.Solid;
            pwMaterial.Diffuse = Color.White;
            pwMaterial.Specular = Color.LightGray;
            pwMaterial.SpecularSharpness = 15f;
            device.Material = pwMaterial;
        }

        private void HandleResetEvent(object caller, EventArgs args)
        {
            CameraPositioning();
            MakePWModel();
        }

        public void InitializeKeyboard()
        {
            keyb = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Keyboard);
            keyb.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            keyb.Acquire();
            mouse = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Mouse);
            mouse.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            mouse.Acquire();
        }

        private void CameraPositioning()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, Width / Height, 1.0f, 100.0f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0f, 0.0f, -5f), new Vector3(0, 0, 0), new Vector3(0.0f, 1.0f, 0.0f));
            device.RenderState.CullMode = Cull.Clockwise;
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Direction = new Vector3(0.8f, 0f, -1f);
            device.Lights[0].Enabled = true;
        }

        private void MakePWModel()
        {
            pwModel = new Mesh[ski.Object.Length];
            pwTexture = new Texture[ski.Object.Length];
            for (int a = 0; a < ski.Object.Length; ++a)
            {
                List<short> list = new List<short>();
                short[] array = new short[ski.Object[a].Faces.Count() * 3];
                for (int i = 0; i < ski.Object[a].Faces.Length; i++)
                {
                    list.Add(ski.Object[a].Faces[i].VertIndexs[0]);
                    list.Add(ski.Object[a].Faces[i].VertIndexs[1]);
                    list.Add(ski.Object[a].Faces[i].VertIndexs[2]);
                }
                array = list.ToArray();
                CustomVertex.PositionNormalTextured[] array2 = new CustomVertex.PositionNormalTextured[ski.Object[a].VertexCount];
                for (int i = 0; i < ski.Object[a].Vertexes.Length; i++)
                {
                    array2[i].Position = ski.Object[a].Vertexes[i].Position;
                    array2[i].Normal = ski.Object[a].Vertexes[i].Normal;
                    array2[i].Tu = ski.Object[a].Vertexes[i].UVCoords[0];
                    array2[i].Tv = ski.Object[a].Vertexes[i].UVCoords[1];
                }
                pwModel[a] = new Mesh(array.Count() / 3, array2.Count(), MeshFlags.Managed, VertexFormats.PositionNormal | VertexFormats.Texture1, device);
                pwModel[a].SetVertexBufferData(array2, LockFlags.None);
                pwModel[a].SetIndexBufferData(array, LockFlags.None);
                int[] adjacencyIn = new int[pwModel[a].NumberFaces];
                try
                {
                    pwModel[a].OptimizeInPlace(MeshFlags.OptimizeVertexCache, adjacencyIn);
                    pwModel[a] = pwModel[a].Clone(pwModel[a].Options.Value, VertexFormats.PositionNormal | VertexFormats.Texture1, device);
                    pwModel[a].ComputeNormals();
                }
                catch
                {

                }
                if (ski.Textures.Length > 0 && ski.Object[a].TexIndex <= ski.Textures.Length && ski.Object[a].TexIndex > -1)
                {
                  //  var texturefile = Form1.pck_entry.Where(x => x.FilePath.StartsWith(Form1.intoPath) && x.FilePath.Contains(ski.Textures[ski.Object[a].TexIndex])).ToList();
                  //  if (texturefile.Count > 0) pwTexture[a] = TextureLoader.FromStream(device, new MemoryStream(Form1.pck.ReadFile(texturefile.First())));
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.DarkGray, 1f, 0);
            device.BeginScene();
            device.Transform.View = Matrix.LookAtLH(new Vector3(0f, 0f, angle), new Vector3(lr, ud, 0f), new Vector3(0f, 1f, 0f));
            device.Transform.World = Matrix.RotationYawPitchRoll(rlr, rud, 0f);
            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.None;
            for (int i = 0; i < pwModel.Count(); i++)
            {
                if (pwTexture[i] != null)
                {
                    device.SetTexture(0, pwTexture[i]);
                }
                int num = pwModel[i].GetAttributeTable().Length;
                for (int j = 0; j < num; j++)
                {
                    pwModel[i].DrawSubset(j);
                }
            }
            device.EndScene();
            device.Present();
            Invalidate();
            ReadKeyboard();
        }

        private void ReadKeyboard()
        {
            KeyboardState currentKeyboardState = keyb.GetCurrentKeyboardState();
            if (Focused)
            {
                if (currentKeyboardState[Key.UpArrow] && currentKeyboardState[Key.RightShift])
                {
                    angle -= 0.5f;
                }
                else if (currentKeyboardState[Key.UpArrow])
                {
                    angle -= 0.1f;
                }
                if (currentKeyboardState[Key.DownArrow] && currentKeyboardState[Key.RightShift])
                {
                    angle += 0.8f;
                }
                else if (currentKeyboardState[Key.DownArrow])
                {
                    angle += 0.1f;
                }
                if (currentKeyboardState[Key.Left])
                {
                    lr += 0.05f;
                }
                if (currentKeyboardState[Key.Right])
                {
                    lr -= 0.05f;
                }
                MouseState currentMouseState = mouse.CurrentMouseState;
                byte[] mouseButtons = currentMouseState.GetMouseButtons();
                if (mouseButtons[0] > 1)
                {
                    if (currentMouseState.X != 0)
                    {
                        lr += currentMouseState.X / 50f;
                    }
                    if (currentMouseState.Y != 0)
                    {
                        ud += currentMouseState.Y / 50f;
                    }
                }
                if (mouseButtons[1] > 1)
                {
                    if (currentMouseState.X != 0)
                    {
                        rlr += currentMouseState.X / 50f;
                    }
                    if (currentMouseState.Y != 0)
                    {
                        rud += currentMouseState.Y / 50f;
                    }
                }
                //if (currentMouseState.Z != 0)
                //{
                //    angle -= currentMouseState.Z / 150f;
                //}
            }
        }

        private void AsModel_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }

    public class SkiReader
    {
        public string[] Bips;

        public string[] Textures;

        public string ModelFilePath
        {
            get;
            set;
        }

        public byte[] Signature
        {
            get;
            set;
        }

        public uint SkiType
        {
            get;
            set;
        }

        public uint[] MeshCount
        {
            get;
            set;
        }

        public uint TexturesCount
        {
            get;
            set;
        }

        public uint MaterialsCount
        {
            get;
            set;
        }

        public uint NumBips
        {
            get;
            set;
        }

        public uint Unknow2
        {
            get;
            set;
        }

        public uint TypeMask
        {
            get;
            set;
        }

        public byte[] UnknowBytes
        {
            get;
            set;
        }

        public SkiMaterial[] Materials
        {
            get;
            set;
        }

        public MeshObject[] Object
        {
            get;
            set;
        }

        public SkiReader(byte[] model)
        {
            this.Bips = new string[0];
            this.Textures = new string[0];
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(model)))
            {
                this.Signature = binaryReader.ReadBytes(8);
                if (this.Signature[0] == 77)
                {
                    if (this.Signature[7] == 65)
                    {
                        this.SkiType = binaryReader.ReadUInt32();
                        this.MeshCount = new uint[4];
                        for (int i = 0; i < 4; i++)
                        {
                            this.MeshCount[i] = binaryReader.ReadUInt32();
                        }
                        this.TexturesCount = binaryReader.ReadUInt32();
                        this.MaterialsCount = binaryReader.ReadUInt32();
                        this.NumBips = binaryReader.ReadUInt32();
                        this.Unknow2 = binaryReader.ReadUInt32();
                        this.TypeMask = binaryReader.ReadUInt32();
                        this.UnknowBytes = binaryReader.ReadBytes(60);
                        if (this.SkiType == 9u)
                        {
                            this.Bips = new string[this.NumBips];
                            int num = 0;
                            while ((long)num < (long)((ulong)this.NumBips))
                            {
                                int count = binaryReader.ReadInt32();
                                byte[] bytes = binaryReader.ReadBytes(count);
                                this.Bips[num] = Encoding.GetEncoding("GB2312").GetString(bytes);
                                num++;
                            }
                        }
                        this.Textures = new string[this.TexturesCount];
                        int num2 = 0;
                        while ((long)num2 < (long)((ulong)this.TexturesCount))
                        {
                            int count2 = binaryReader.ReadInt32();
                            byte[] bytes2 = binaryReader.ReadBytes(count2);
                            this.Textures[num2] = Encoding.GetEncoding("GB2312").GetString(bytes2).Replace(".DDS", ".dds");
                            num2++;
                        }
                        this.Materials = new SkiMaterial[this.MaterialsCount];
                        int num3 = 0;
                        while ((long)num3 < (long)((ulong)this.MaterialsCount))
                        {
                            this.Materials[num3] = SkiMaterial.Read(binaryReader);
                            num3++;
                        }
                        if (this.MeshCount[0] != 0u)
                        {
                            this.Object = new MeshObject[this.MeshCount[0]];
                            int num4 = 0;
                            while ((long)num4 < (long)((ulong)this.MeshCount[0]))
                            {
                                this.Object[num4] = MeshObject.Read(binaryReader, 0);
                                num4++;
                            }
                        }
                        else
                        {
                            this.Object = new MeshObject[1];
                            this.Object[0] = MeshObject.Read(binaryReader, 1);
                        }
                        return;
                    }
                }
                throw new Exception("Its no ski format");
            }
        }
    }

    public class SkiMaterial
    {
        public byte TrailZero
        {
            get;
            set;
        }

        public float[] MaterialParamsA
        {
            get;
            set;
        }

        public float[] MaterialParamsB
        {
            get;
            set;
        }

        public float[] MaterialParamsC
        {
            get;
            set;
        }

        public float[] MaterialParamsD
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public byte Clothing
        {
            get;
            set;
        }

        public static SkiMaterial Read(BinaryReader r)
        {
            r.ReadBytes(10);
            SkiMaterial skiMaterial = new SkiMaterial()
            {
                TrailZero = r.ReadByte(),
                MaterialParamsA = new float[4],
                MaterialParamsB = new float[4],
                MaterialParamsC = new float[4],
                MaterialParamsD = new float[4]
            };
            for (int i = 0; i < 4; i++)
            {
                skiMaterial.MaterialParamsA[i] = r.ReadSingle();
            }
            for (int j = 0; j < 4; j++)
            {
                skiMaterial.MaterialParamsB[j] = r.ReadSingle();
            }
            for (int k = 0; k < 4; k++)
            {
                skiMaterial.MaterialParamsC[k] = r.ReadSingle();
            }
            for (int l = 0; l < 4; l++)
            {
                skiMaterial.MaterialParamsD[l] = r.ReadSingle();
            }
            skiMaterial.Scale = r.ReadSingle();
            skiMaterial.Clothing = r.ReadByte();
            return skiMaterial;
        }
    }

    public class Vertex
    {
        public Vector3 Position
        {
            get;
            set;
        }

        public float[] VertexWeight
        {
            get;
            set;
        }

        public byte[] BoneIndex
        {
            get;
            set;
        }

        public Vector3 Normal
        {
            get;
            set;
        }

        public float[] UVCoords
        {
            get;
            set;
        }

        public static Vertex Read(BinaryReader r, int vertex_type = 0)
        {
            Vertex vertex = new Vertex()
            {
                Position = new Vector3
                {
                    X = r.ReadSingle(),
                    Y = r.ReadSingle(),
                    Z = r.ReadSingle()
                }
            };
            if (vertex_type == 0)
            {
                vertex.VertexWeight = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    vertex.VertexWeight[i] = r.ReadSingle();
                }
                vertex.BoneIndex = new byte[4];
                for (int j = 0; j < 4; j++)
                {
                    vertex.BoneIndex[j] = r.ReadByte();
                }
            }
            vertex.Normal = new Vector3
            {
                X = r.ReadSingle(),
                Y = r.ReadSingle(),
                Z = r.ReadSingle()
            };
            vertex.UVCoords = new float[2] { r.ReadSingle(), r.ReadSingle() };
            return vertex;
        }
    }

    public class Face
    {
        public short[] VertIndexs
        {
            get;
            set;
        }

        public static Face Read(BinaryReader r)
        {
            Face face = new Face()
            {
                VertIndexs = new short[3]
            };
            for (int i = 0; i < 3; i++)
            {
                face.VertIndexs[i] = r.ReadInt16();
            }
            return face;
        }
    }

    public class MeshObject
    {
        public string Name
        {
            get;
            set;
        }

        public int TexIndex
        {
            get;
            set;
        }

        public int MatIndex
        {
            get;
            set;
        }

        public uint VertexCount
        {
            get;
            set;
        }

        public uint IndexCount
        {
            get;
            set;
        }

        public Vertex[] Vertexes
        {
            get;
            set;
        }

        public Face[] Faces
        {
            get;
            set;
        }

        public static MeshObject Read(BinaryReader r, int vertex_type = 0)
        {
            MeshObject meshObject = new MeshObject();
            int count = r.ReadInt32();
            byte[] bytes = r.ReadBytes(count);
            meshObject.Name = Encoding.GetEncoding("GB2312").GetString(bytes);
            meshObject.TexIndex = r.ReadInt32();
            meshObject.MatIndex = r.ReadInt32();
            if (vertex_type == 1)
            {
                r.ReadInt32();
            }
            meshObject.VertexCount = r.ReadUInt32();
            meshObject.IndexCount = r.ReadUInt32();
            meshObject.Vertexes = new Vertex[meshObject.VertexCount];
            int num = 0;
            while ((long)num < (long)((ulong)meshObject.VertexCount))
            {
                meshObject.Vertexes[num] = Vertex.Read(r, vertex_type);
                num++;
            }
            uint num2 = meshObject.IndexCount / 3u;
            meshObject.Faces = new Face[num2];
            int num3 = 0;
            while ((long)num3 < (long)((ulong)num2))
            {
                meshObject.Faces[num3] = Face.Read(r);
                num3++;
            }
            return meshObject;
        }
    }
}