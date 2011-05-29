
namespace Sunfish.TagLayouts
{
    public class ant : TagDefinition
    {
        public ant()
        {
            Size = 160;
            Values = new Value[] {
                new Data(152),
                new TagBlockArray(new TagBlock(128, null)),
            };
        }
    }
}