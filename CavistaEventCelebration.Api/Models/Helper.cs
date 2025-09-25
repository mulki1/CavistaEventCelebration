namespace CavistaEventCelebration.Api.Models
{
    public static class Helper
    {
        private static Random rand = new Random();

        public static string GeneratePassword()
        {
            var specialCharacters = new List<string>{ "@", "_", "-", "&", "%", "?", "#", "$"};
            var integers = new List<int>{ 1,2,3,4,5,6,7,8,9,0};
            var alphabets = new List<string>{"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p","q","r","s","t","u","v","w","x","y","z"};
            var newPassword = new List<string>();
  
            for (var val = 0; val < 10; val++) 
            {
               if (val < 2)
                {
                    var toAdd = specialCharacters[rand.Next(7)];
                    newPassword.Add(toAdd.ToString());
                } 
                else if (val < 6)
                {
                    var toAdd = integers[rand.Next(9)];
                    newPassword.Add(toAdd.ToString());  
                }
               else if (val < 7)
                {
                    var toAdd = alphabets[rand.Next(25)];
                    newPassword.Add(toAdd.ToUpper());
                }
                else
                {
                    var toAdd = alphabets[rand.Next(25)];
                    newPassword.Add(toAdd.ToString());
                }
            }

            newPassword.Shuffle();


            return string.Join("", newPassword);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
