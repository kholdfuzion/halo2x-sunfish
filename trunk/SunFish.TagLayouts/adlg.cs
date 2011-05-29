
namespace Sunfish.TagLayouts
{
    public class adlg : TagDefinition
    {
        public adlg()
        {
            Size = 44;
            Values = new Value[] {
                new TagBlockArray(new TagBlock(96, new Value[]{
                    new Data(80),
                    new TagBlockArray(new TagBlock(12, null)),
                })),
                new TagBlockArray(new TagBlock(64, null)),
                new Data(12),
                new TagBlockArray(new TagBlock(4, null)),
                new TagBlockArray(new TagBlock(4, null)),
            };
        }
    }
}