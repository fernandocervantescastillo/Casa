using System;
using System.Collections.Generic;
using System.Text;
using Casa.common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Casa
{
    class Trapecio
    {
       
        float[] _vertices;
        uint[] _indices;

        public void init(float a, float l, float h, float x, float y, float z)
        {
            _vertices[0] = 0.0f+x; _vertices[1] = 0.0f + y; _vertices[2] = 0.0f + z;
            _vertices[3] = l + x; _vertices[4] = 0.0f + y; _vertices[5] = 0.0f + z;
            _vertices[6] = 0.0f + x; _vertices[7] = 0.0f + y; _vertices[8] = a + z;
            _vertices[9] = l + x; _vertices[10] = 0.0f + y; _vertices[11] = a + z;
            _vertices[12] = 0.0f + x; _vertices[13] = h + y; _vertices[14] = a/2.0f + z;
            _vertices[15] = l + x; _vertices[16] = h + y; _vertices[17] = a/2.0f + z;

            _indices[0] = 1; _indices[1] = 0; _indices[2] = 5;
            _indices[3] = 1; _indices[4] = 5; _indices[5] = 4;
            _indices[6] = 3; _indices[7] = 1; _indices[8] = 5;
            _indices[9] = 0; _indices[10] = 2; _indices[11] = 4;
            _indices[12] = 2; _indices[13] = 3; _indices[14] = 5;
            _indices[15] = 2; _indices[16] = 5; _indices[17] = 4;
            _indices[18] = 0; _indices[19] = 1; _indices[20] = 3;
            _indices[21] = 0; _indices[22] = 3; _indices[23] = 2;
        }

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;

        private Shader _shader;


        public Trapecio(float a, float l, float h, float x, float y, float z)
        {

            _vertices = new float[18];
            _indices = new uint[24];
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

        public void render(double _time, Camera _camera)
        {
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
