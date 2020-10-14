using System;

class Program {
    static public void Main() {
        Console.Clear();
        Console.CursorVisible = false;
        TBA.Selector selector = new TBA.Selector();
        TBA.Stage stage;
        int output = 0;

        selector.Clear();
        selector.AddRange(new string[]{
            "Adam",
            "Fredrik",
            "Johan"
        });

        stage = new TBA.Stage(new string[]{
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque lacinia in augue at ullamcorper.",
            "Vivamus est dolor, egestas ac libero sit amet, vulputate semper odio.",
            "Proin porta aliquam lectus quis molestie."
        },selector);
        output = stage.Run();
        Console.WriteLine(output);

    }
}