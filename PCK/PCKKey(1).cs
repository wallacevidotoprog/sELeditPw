﻿
namespace sELedit.PCK
{
    public class PCKKey
    {
        public int KEY_1 = -1466731422;
        public int KEY_2 = -240896429;
        public int ASIG_1 = -33685778;
        public int ASIG_2 = -267534609;
        public int FSIG_1 = 1305093103;
        public int FSIG_2 = 1453361591;

        public PCKKey()
        {
        }

        public PCKKey(int key1, int key2)
        {
            this.KEY_1 = key1;
            this.KEY_2 = key2;
        }

        public PCKKey(int key1, int key2, int asig1, int asig2, int fsig1, int fsig2)
        {
            this.KEY_1 = key1;
            this.KEY_2 = key2;
            this.ASIG_1 = asig1;
            this.ASIG_2 = asig2;
            this.FSIG_1 = fsig1;
            this.FSIG_2 = fsig2;
        }
    }
}
