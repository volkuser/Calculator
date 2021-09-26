using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System;
using NCalc;

namespace WPF_Calculator
{
    public partial class MainWindow : Window
    {
        //----------//
        /* checkers */
        //----------//
        // for definition of calculator mode
        string mode = "standard";
        /* for completeness of a floating
         * point number */
        bool isFloated = false;
        // for number is powed
        bool isPowed = false;
        // for valid of close bracket
        int bracketCounter = 0;
        bool firstExpression = true;

        //-------------//
        /* basic stuff */
        //-------------//
        // current expression
        string expression = string.Empty;
        // corrent addition to expression
        string addition = string.Empty;
        /* displaying data (expression and result)
         * in a label */
        string displayExpression = string.Empty;
        // result after pressing the "=" button
        double result = 0;
        // result for programming mode
        string hex = string.Empty,
            oct = string.Empty,
            bin = string.Empty;

        //-------------//
        /* some things */
        //-------------//
        /* dictionary with operations
         * for program perception of them */
        private Dictionary<string, string> Operations
            = new Dictionary<string, string>();
        // array with buttons of engineering mode
        Button[] engineeringButtons = new Button[17];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // adding operation in dictionary
            Operations.Add("+", "+");
            Operations.Add("-", "-");
            Operations.Add("x", "*");
            Operations.Add("÷", "/");

            // initialization array with engineering buttons
            byte counter = 0; // button index in this array 
            foreach (var element in MainGrid.Children)
            {
                if (element is Button)
                {
                    // initialization array with buttons
                    // from engineering mode
                    Button btn_temp = (Button)element;
                    // originally this button is hidden
                    // because they are written in array
                    // precisely on this parameter
                    if (btn_temp.Visibility == Visibility.Hidden)
                    {
                        engineeringButtons[counter] = btn_temp;
                        counter++;
                    }
                }
            }
        }

        //------------------//
        /* changing of mode */
        //------------------//
        // return standard mode
        private void smode_Click(object sender, RoutedEventArgs e)
        {
            // return title
            mainWindow.Title = "Standard Calculator";
            // return if there was engineering mode
            if (mode == "engineering")
            {
                // hiding elements of engineering mode
                foreach (var element in MainGrid.Children)
                {
                    if (element is Button)
                    {
                        Button btn_temp = (Button)element;
                        foreach (Button button in engineeringButtons)
                            if (btn_temp == button)
                                btn_temp.Visibility = Visibility.Hidden;
                    }
                }
                // return decired grid layout of standard buttons 
                foreach (var element in MainGrid.Children)
                {
                    if (element is Button)
                    {
                        Button btn_temp = (Button)element;
                        if (btn_temp.Visibility == Visibility.Visible)
                        {
                            Grid.SetRow((UIElement)element,
                                Grid.GetRow((UIElement)element) - 2);
                            Grid.SetColumn((UIElement)element,
                                Grid.GetColumn((UIElement)element) - 1);
                        }
                    }
                }
                /* removing unnecessary columns and rows of main grid
                 * and resize them */
                MainGrid.ColumnDefinitions.RemoveAt(4);
                mainWindow.MinWidth = 300;
                if (mainWindow.Width >= 300 + 300 / 5)
                    mainWindow.Width -= mainWindow.Width / 5;
                for (int i = 8; i > 6; i--)
                    MainGrid.RowDefinitions.RemoveAt(i);
                mainWindow.MinHeight = 400;
                if (mainWindow.Height >= 400 + 400 / 9 * 2)
                    mainWindow.Height -= mainWindow.Height / 9 * 2;
            }
            else if (mode == "programming")
            {
                // clearing of textbox
                expression = addition = string.Empty;
                LabelPanel.Text = "0";
                result = 0;
                // return standard font size
                LabelPanel.FontSize = 30;
                // hiding buttons with brackets
                btn_OpenBracket.Visibility
                    = btn_CloseBracket.Visibility
                    = Visibility.Hidden;
                // return standard grid layout for textbox
                Grid.SetColumnSpan(LabelPanel, 4);
                // return snandard width, minwidth and others
                mainWindow.MinWidth = 300;
                if (mainWindow.Width >= 300 + 50)
                    mainWindow.Width -= 50;
                mainWindow.MinHeight = 400;
                if (mainWindow.Height >= 400 + 80)
                    mainWindow.Height -= 80;
                /* return ordinary size of length
                 * of height of secondrow of grid */
                firstRow.Height = new GridLength(1, GridUnitType.Star);
            }
            // indecation that mode has been changed on standard
            mode = "standard";
        }
        // enable engineering mode
        private void emode_Click(object sender, RoutedEventArgs e)
        {
            // change title
            mainWindow.Title = "Engineering Calculator";
            if (mode != "engineering")
            {
                /* adding new columns and rows in grid 
                 * and change of size */
                if (mainWindow.Width <= (300 + 300 / 4))
                    mainWindow.Width = 300 + 300 / 4;
                mainWindow.MinWidth = 300 + 300 / 4;
                ColumnDefinition newColumn = new ColumnDefinition();
                MainGrid.ColumnDefinitions.Add(newColumn);
                if (mainWindow.Height <= (400 + 400 / 9 * 2))
                    mainWindow.Height = 400 + 400 / 9 * 2;
                mainWindow.MinHeight = 400 + 400 / 9 * 2;
                for (int i = 0; i < 2; i++)
                {
                    RowDefinition newRow = new RowDefinition();
                    MainGrid.RowDefinitions.Add(newRow);
                }
                foreach (var element in MainGrid.Children)
                {
                    if (element is Button)
                    {
                        Button btn_temp = (Button)element;
                        // turning on engineering buttons display
                        if (btn_temp.Visibility == Visibility.Hidden)
                            btn_temp.Visibility = Visibility.Visible;
                        else if (btn_temp == btn_OpenBracket ||
                            btn_temp == btn_CloseBracket)
                            Grid.SetColumn((UIElement)element,
                                Grid.GetColumn((UIElement)element) + 1);
                        else
                        {
                            /* moving standard buttons
                             * to right lower corner */
                            Grid.SetRow((UIElement)element,
                                Grid.GetRow((UIElement)element) + 2);
                            Grid.SetColumn((UIElement)element,
                                Grid.GetColumn((UIElement)element) + 1);
                        }
                    }
                }
                if (mode == "programming")
                {
                    // clearing of textbox
                    expression = addition = string.Empty;
                    LabelPanel.Text = "0";
                    result = 0;
                    // return standard grid layout for textbox
                    Grid.SetColumnSpan(LabelPanel, 4);
                    /* return ordinary size of length
                     * of height of secondrow of grid */
                    firstRow.Height = new GridLength(1, GridUnitType.Star);
                }
            }
            // indecation that mode has been changed on engineering
            mode = "engineering";
        }
        // enable programming mode
        private void pmode_Click(object sender, RoutedEventArgs e)
        {
            // change title
            mainWindow.Title = "Programming Calculator";
            // change font size in textbox
            LabelPanel.FontSize = 20;
            // change grid layout for textbox
            Grid.SetColumnSpan(LabelPanel, 3);
            // new length for main grid dirst row
            firstRow.Height = new GridLength(2, GridUnitType.Star);
            if (mode == "standard")
            {
                // resize main grid
                if (mainWindow.Width <= 300 + 50)
                    mainWindow.Width = 300 + 50;
                mainWindow.MinWidth = 300 + 50;
                if (mainWindow.Height <= 400 + 80)
                    mainWindow.Height = 400 + 80;
                mainWindow.MinHeight = 400 + 80;
            }
            else if (mode == "engineering")
            {
                // hiding elements of engineering mode
                foreach (var element in MainGrid.Children)
                {
                    if (element is Button)
                    {
                        Button btn_temp = (Button)element;
                        foreach (Button button in engineeringButtons)
                            if (btn_temp == button)
                                btn_temp.Visibility = Visibility.Hidden;
                    }
                }
                // return decired grid layout of standard buttons 
                foreach (var element in MainGrid.Children)
                {
                    if (element is Button)
                    {
                        Button btn_temp = (Button)element;
                        if (btn_temp.Visibility == Visibility.Visible)
                        {
                            Grid.SetRow((UIElement)element,
                                Grid.GetRow((UIElement)element) - 2);
                            Grid.SetColumn((UIElement)element,
                                Grid.GetColumn((UIElement)element) - 1);
                        }
                    }
                }

                // removing unnecessary columns and rows of main grid
                MainGrid.ColumnDefinitions.RemoveAt(4);
                for (int i = 8; i > 6; i--)
                    MainGrid.RowDefinitions.RemoveAt(i);

                // resize columns and rows of main grid
                mainWindow.MinWidth = 300 + 50;
                mainWindow.Width -= 50;
                mainWindow.MinHeight = 400 + 80;
                mainWindow.Height -= 80;
            }
            // adding buttons with brackets
            btn_OpenBracket.Visibility
                = btn_CloseBracket.Visibility
                = Visibility.Visible;

            /* indecation that mode has been
             * changed on programming */
            mode = "programming";
        }

        //---------------------------------------------//
        /* buttons for all calculator modes (standard) */
        //---------------------------------------------//
        // handler for numbers and dot
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            // get clicked button with number or dot
            Button button = (Button)sender;

            // if button of dot
            if (button.Content.ToString() == ".")
                if (!isFloated)
                    isFloated = true;
                else return;

            // button content write to visible expression
            addition += button.Content;
            Update_LabelPanel();
        }
        // handler for standard operations
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            expression +=
                $"{addition}{Operations[((Button)sender).Content.ToString()]}";
            addition = string.Empty;
            isFloated = false;
            Update_LabelPanel();
        }
        // changed plus on minus or adding minus to number
        private void btn_Sign_Click(object sender, RoutedEventArgs e)
        {
            // if number is null
            if (addition.Length == 0) return;
            // if number is negative
            if (addition[0] == '-') addition = addition.Substring(1);
            // null case handling
            else if (addition[0] != '0')
                addition = addition.Insert(0, "-");
            Update_LabelPanel();
        }

        //-----------------------//
        /* functions of removing */
        //-----------------------//
        // backspace
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            // if there is addition
            if (addition.Length >= 1)
            {
                if (addition[addition.Length - 1] == '.')
                    isFloated = false;

                addition = addition.Remove(addition.Length - 1, 1);
            }
            // if there isn't addition and there is expression
            else if (expression.Length >= 1)
            {
                addition = string.Empty;
                expression = expression.Remove(expression.Length - 1, 1);
            }
            Update_LabelPanel();
        }
        // clear expression (and addition)
        private void btn_CE_Click(object sender, RoutedEventArgs e)
        {
            expression = addition = string.Empty;
            isFloated = false;
            Update_LabelPanel();
        }
        // clear all
        private void btn_C_Click(object sender, RoutedEventArgs e)
        {
            expression = addition = string.Empty;
            result = 0;
            isFloated = false;
            Update_LabelPanel();
        }

        //----------------------------------------//
        /* brackets (header for clicked on buttons*/
        //----------------------------------------//
        private void Brackets_Click(object sender, RoutedEventArgs e)
        {
            // getting current bracket as string
            string currentBracket = ((Button)sender).Content.ToString();
            // block for closing bracket
            if (bracketCounter >= 0 && currentBracket == ")") { return; }

            // counting counter
            if (currentBracket == "(")
                bracketCounter -= 1;
            else if (currentBracket == ")")
                bracketCounter += 1;

            expression += currentBracket == ")"
                ? $"{addition}{currentBracket}" : $"{currentBracket}";
            addition = string.Empty;
            Update_LabelPanel();
        }

        //-----------------------------------//
        /* buttons of only engineering mode */
        //----------------------------------//
        private void Trigonometry_Click(object sender, RoutedEventArgs e)
        {
            addition = $"{((Button)sender).Content}({addition})";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_Factorial_Click(object sender, RoutedEventArgs e)
        {
            expression += $"fact({addition})";
            addition = string.Empty;
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_PI_Click(object sender, RoutedEventArgs e)
        {
            addition += $"pi";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_E_Click(object sender, RoutedEventArgs e)
        {
            expression += $"exp({addition})";
            addition = string.Empty;
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_Pow2_Click(object sender, RoutedEventArgs e)
        {
            addition += $"Pow({addition}, 2)";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_Fraction_Click(object sender, RoutedEventArgs e)
        {
            addition = $"Pow({addition}, -1)";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_Sqrt_Click(object sender, RoutedEventArgs e)
        {
            addition = $"Sqrt({addition})";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_Module_Click(object sender, RoutedEventArgs e)
        {
            addition = $"Abs({addition})";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_PowY_Click(object sender, RoutedEventArgs e)
        {
            if (!isPowed)
            {
                expression += $"Pow({addition},";
                isPowed = true;
            }
            else
            {
                expression += $"{addition})";
                isPowed = false;
            }
            addition = string.Empty;
            Update_LabelPanel();
        }
        private void btn_Pow10_Click(object sender, RoutedEventArgs e)
        {
            addition = $"Pow(10, {addition})";
            isFloated = false;
            Update_LabelPanel();
        }
        private void btn_Log_Click(object sender, RoutedEventArgs e)
        {
            if (!isPowed)
            {
                expression += $"Log({addition},";
                isPowed = true;
            }
            else
            {
                expression += $"{addition})";
                isPowed = false;
            }
            addition = string.Empty;
            Update_LabelPanel();
        }
        private void btn_Ln_Click(object sender, RoutedEventArgs e)
        {
            addition = $"Ln({addition})";
            isFloated = false;
            Update_LabelPanel();
        }

        //------------------------------//
        /* getting result of expression */
        //------------------------------//
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            expression += addition; // last addition is added to expression
            // if expression is empty
            if (expression == string.Empty) return;
            NCalc.Expression ncalcExpession; // creating object of NCalc
            // converting string expression to double expression
            /* check first symbol of expression 
             * on relation to decimal number system
             * or to the Latin alphabet */
            if (Char.IsDigit(expression[0]) || Char.IsLetter(expression[0]))
                ncalcExpession = new NCalc.Expression(expression);
            else
            {
                // replae points on dots
                // result added to expression as substring
                string temp = result.ToString().Replace(",", ".");
                if (!firstExpression) ncalcExpession =
                        new NCalc.Expression($"{temp} {expression}");
                else ncalcExpession = new NCalc.Expression(expression);
            }
            // if number pi
            ncalcExpession.Parameters["pi"] = Math.PI;
            // if special functions
            ncalcExpession.EvaluateFunction += delegate (string name, FunctionArgs args)
            {
                switch (name)
                {
                    case "fact":
                        double tmp = 1, parameter =
                        Convert.ToDouble(args.Parameters[0].Evaluate());
                        for (int i = 1; i <= parameter; i++) tmp *= i;
                        args.Result = tmp;
                        break;
                    case "exp":
                        args.Result = Math.Exp(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                    case "Ln":
                        args.Result = Math.Log(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                }
            };
            try
            {
                result = Convert.ToDouble(ncalcExpession.Evaluate());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                expression =
                    addition =
                    string.Empty;
            }

            if (mode == "programming")
            {
                hex = Convert.ToString((int)result, 16);
                oct = Convert.ToString((int)result, 8);
                bin = Convert.ToString((int)result, 2);
            }

            // line emptying with expression
            expression = addition = string.Empty;
            isFloated = false; // number added
            firstExpression = false;
            Update_LabelPanel();
        }

        //------------------------//
        /* updating the displayed */
        //------------------------//
        private void Update_LabelPanel()
        {
            if (!firstExpression)
                displayExpression = $"{result}\n{expression}{addition}";
            else displayExpression = $"\n{expression}{addition}";
            if (mode == "programming")
            {
                displayExpression += $"\nHEX: {hex}";
                displayExpression += $"\nDEC: {result}";
                displayExpression += $"\nOCT: {oct}";
                displayExpression += $"\nBIN: {bin}";
            }
            LabelPanel.Text = displayExpression;
        }
    }
}
