
namespace Sunfish.TagLayouts
{
    public class bitm : TagDefinition
    {
        public bitm()
        {
            Size = 76;
            Values = new Value[] {
                new Data(60),
                new TagBlockArray(new TagBlock(60, new Value[]{
                    new Data(52),
                    new TagBlockArray(new TagBlock(32, null)),
                })),
                new TagBlockArray(new TagBlock(116, new Value[]{
                    new RawBlock(28, 52),
                    new RawBlock(32, 56),
                    new RawBlock(36, 60),
                    new RawBlock(40, 64),
                    new RawBlock(44, 68),
                    new RawBlock(48, 72),
                })),
            };
        }
    }
}