   M S B A M L   `   `   `  ����@  <Lab 7, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:nicorueda	nicorueda  5   	       nicorueda.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:nicorueda     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
      �� $��Hours��6   $
��Left=�6   $��	10,10,0,0q�63   $	��Top=�6F   �� 5      $��Minutes��6   $
��Left=�6   $��	10,36,0,0q�65   $	��Top=�6H   �� 5      $��Seconds��6	   $
��Left=�6   $��9,62,0,0q�66   $	��Top=�6H   �� 5      -   5         ��Name$ HoursTextBox��5      $
��Left=�6!   $��23��6<   $��	87,10,0,0q�6H    �� TextWrapping$
 Wrap=�6[   $��0��5      $	��Top=�6   $	��120��6#   6\   �� 5      -   5        $ MinutesTextBox��5      $
��Left=�6#   $��23��6>   $��	87,36,0,0q�6J   $
 Wrap=�6]   $��0��5      $	��Top=�6   $	��120��6#   6[   �� 5      -   5        $ SecondsTextBox��5      $
��Left=�6#   $��23��6>   $��	87,62,0,0q�6J   $
 Wrap=�6]   $��0��5      $	��Top=�6   $	��120��6#   6\   �� 5      -   5         ��Name$ ResultLabel��5   	   $,��&Click Calculate to work out the result��6   $
��Left=�6O   $��	45,93,0,0q�5      $	��Top=�6   6Y   �� 5      -   5         ��Name$ button��5   	   $��	Calculate��6   $
��Left=�6-   $��152,293,0,0q�6H   $	��Top=�6p   5      5      