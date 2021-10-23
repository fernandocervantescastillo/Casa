using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Casa.common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Casa
{
    class Cubo
    {

        float[] _vertices;
        uint[] _indices;

        void init(float a, float l, float h, float x, float y, float z)
        {
            _vertices[0]= 0.0f+x; _vertices[1] = 0.0f+y; _vertices[2] = 0.0f+z;
            _vertices[3] = l + x; _vertices[4] = 0.0f + y; _vertices[5] = 0.0f + z;
            _vertices[6] = 0.0f + x; _vertices[7] = 0.0f + y; _vertices[8] = a + z;
            _vertices[9] = l + x; _vertices[10] = 0.0f + y; _vertices[11] = a + z;
            _vertices[12] = 0.0f + x; _vertices[13] = h + y; _vertices[14] = 0.0f + z;
            _vertices[15] = l + x; _vertices[16] = h + y; _vertices[17] = 0.0f + z;
            _vertices[18] = 0.0f + x; _vertices[19] = h + y; _vertices[20] = a + z;
            _vertices[21] = l + x; _vertices[22] = h + y; _vertices[23] = a + z;

            _indices[0]= 1; _indices[1] = 0; _indices[2] = 4;
            _indices[3] = 1; _indices[4] = 4; _indices[5] = 5;
            _indices[6] = 3; _indices[7] = 1; _indices[8] = 5;
            _indices[9] = 3; _indices[10] = 5; _indices[11] = 7;
            _indices[12] = 0; _indices[13] = 2; _indices[14] = 6;
            _indices[15] = 0; _indices[16] = 6; _indices[17] = 4;
            _indices[18] = 0; _indices[19] = 1; _indices[20] = 3;
            _indices[21] = 0; _indices[22] = 3; _indices[23] = 2;
            _indices[24] = 6; _indices[25] = 7; _indices[26] = 5;
            _indices[27] = 6; _indices[28] = 5; _indices[29] = 4;
            _indices[30] = 2; _indices[31] = 3; _indices[32] = 7;
            _indices[33] = 2; _indices[34] = 7; _indices[35] = 6;
        }

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;

        private Shader _shader;


        public Cubo(float a, float l, float h, float x, float y, float z)
        {
            _vertices = new float[24];
            _indices = new uint[36];
            init(a,l,h,x,y,z);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            
        }

        public void render(double _time, Camera _camera) {
            GL.BindVertexArray(_vertexArrayObject);
            _shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void dispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shader.Handle);
        }


    }
}
