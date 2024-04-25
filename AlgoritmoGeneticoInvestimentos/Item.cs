partial class Program
{
    class Item
    {
        public string Nome { get; set; }
        public int Peso { get; set; }
        public int Valor { get; set; }

        public Item(string nome, int peso, int valor)
        {
            Nome = nome;
            Peso = peso;  
            Valor = valor; //Importancia 
        }
    }

}
