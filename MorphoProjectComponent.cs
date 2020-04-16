using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace MorphoProject
{
    public class MeshRationalizer : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MeshRationalizer()
          : base("RationalizeMesh", "MRational",
              "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.",
              "PanelizationTools", "PanelizationTools")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
           
            pManager.AddNumberParameter("radius", "r", "sphere radius", GH_ParamAccess.item);
            pManager.AddNumberParameter("startHeight", "h", "height to start division from", GH_ParamAccess.item);
            pManager.AddMeshParameter("inputMesh", "m", "input a mesh", GH_ParamAccess.item);
          
        }

      
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
          
            pManager.AddCurveParameter("Line", "ln", "intersection curve", GH_ParamAccess.item);
            pManager.AddLineParameter("intersections", "inters", "intersection curves", GH_ParamAccess.list);


        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double radius = 0.5;
            double startHeight = radius;
            Mesh inputMesh=new Mesh();
            Polyline intersectionLine;
            List<Line> sphInters;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref radius)) return;
            if (!DA.GetData(1, ref startHeight)) return;
            if (!DA.GetData(2, ref inputMesh)) return;
            

            intersectionLine=FirstIntersection(radius, startHeight, inputMesh);
            sphInters = SphIntersections(radius, inputMesh, intersectionLine);
            DA.SetData(0, intersectionLine);
            DA.SetDataList(1, sphInters);
        }

        private Polyline FirstIntersection(double radius, double startHeight, Mesh inputMesh)
        {
            Plane plane = new Plane(new Point3d(0, 0, startHeight), Vector3d.ZAxis); //I will use this plane to intersect it with the input mesh
            Polyline[] firstLine = Intersection.MeshPlane(inputMesh, plane);
           
           
            return firstLine[0];
        }

        private List<Line> SphIntersections(double radius, Mesh mesh,Polyline pathPoly)
        {
            List<Line> plns = new List<Line>();
            Curve path = pathPoly.ToNurbsCurve();
            int numOfSpheres = (int)(path.GetLength()/(2*radius)); //how many spheres along the path
            Point3d center = path.PointAt(0);

                Sphere sph = new Sphere(center, radius);
                Brep sphB = sph.ToBrep();

                Curve[] crvs;
                Point3d[] pts;
                Intersection.CurveBrep(path, sphB, 1.0, out crvs, out pts);
                center = pts[0];
                var sphM = Mesh.CreateFromSphere(sph,5,5);
                Line[] intersM=Intersection.MeshMeshFast(mesh, sphM);

            for (int j = 0; j < intersM.Length; j++)
            {
                plns.Add(intersM[j]);
            }
                
            


            return plns;        
        }

       
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
      
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }
       
        public override Guid ComponentGuid
        {
            get { return new Guid("82785197-afcc-4cca-9cd7-3c9ac4bb1058"); }
        }
    }
}
