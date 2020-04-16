using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace MorphoProject
{
    public class MorphoProjectInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "MorphoProject";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("9d61df8c-a325-4b81-83bf-12dd6fec032c");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
