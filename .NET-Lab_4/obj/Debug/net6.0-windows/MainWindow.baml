   M S B A M L   `   `   `  ����C  ?PT-Lab_4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PT_Lab_4PT_Lab_4  5   	       PT_Lab_4.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:PT_Lab_4     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��450��6)   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5        $��dataGridView1��5
      $��Center=�6(   $	��346��6E   $��0,39,0,0q�6R   $	��Top=�5      $	��430��6+      CanUserResizeColumns$ False��67      AutoGenerateColumns$ False��5         CanUserAddRows$
 True��6/      CanUserReorderColumns$ False��6E      CanUserSortColumns$ False��5         Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5         CellTemplate 5      �� 5      .R�5      �� -   5        
������ $��0q�5   T   5      5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Model��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 64   �� 
 �� Path$
 model�  5      $ Motor��6)    5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6)   .+�5   "   �� $��0,0,5,0q�6N   ��6,   �� $
 motor.model�6N   �� 5   "   $��0,0,5,0q�6U   ��6,   �� $
 motor.displacement�6U   �� 5   "   $��0,0,5,0q�6l   ��6,   �� $
 motor.horsePower� �� StringFormat$ ({0} hp)��6l   5       5!         CellEditingTemplate 5#      �� 5$      .R�5%      �� $ 
Horizontal=�6)   .+�5&   "   �� $��0,0,5,0q�6L   ��6*   �� $
 motor.model�6L   �� 5'   "   $��0,0,5,0q�6S   ��6*   �� $
 motor.displacement�6S   �� 5(   "   $��0,0,5,0q�6Q   ��6*   �� $
 motor.horsePower�6Q   5)      5*      5,        5-      $
 Year��6%   	 63   �� $

 year� � 	Resources 51      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType)  #System.Windows.Controls.DataGridRow �� Triggers �� Binding �� Value �� Property �� Value��%�   '        '  ;     �� 52      "  6   .�53      -   5        54      �� 56      "  6    57      �� 58       6&   �� *
isMatching+ true6E   .a�59      ��  � ��
 ��  �6;   5:      5<      5=      5@      c� 5A   
   $
��Left=�6   $��30��6-   $	��Top=�69   $	��782��6Q   .@�5B      �� $��25��6   $��0q�6    $	��Top=�6+   $��80��6C   $��Search for:��6N   �� 5C      -   5        $��searchTextBox��5C   3   $��0,2,0,1q�6   $	��120��6'   6J   �� 5E      $��
Search in:��6   $��25��6)   $	��Top=�65   �� 5F      -   5        $��comboBox��5F   R   $��25��6   $��0q�6#   $	��Top=�6.   $	��120��6F   6R   �� 5H      -   5        $
��Find��5H      $��30��6$   $��54��60   6;   5I      5J      5K      