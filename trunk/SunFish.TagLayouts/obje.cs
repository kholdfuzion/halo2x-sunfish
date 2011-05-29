
namespace Sunfish.TagLayouts
{
    public class obje : TagDefinition
    {
        public obje()
        {
            Size = 188;
            Values = new Value[] {
                new Data(92),
                new TagBlockArray(new TagBlock(16, null)),
                new TagBlockArray(new TagBlock(32, new Value[] { 
                    new Data(20),
                    new TagBlockArray(new TagBlock(1, null)),
                })),
                new Data(40),
                new TagBlockArray(new TagBlock(24, null)),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(16, new Value[] {
                    new TagBlockArray(new TagBlock(32, null)),
                    new TagBlockArray(new TagBlock(40, null)),
                })),
                new TagBlockArray(new TagBlock(8, null)),
            };
        }
    }
}