   M S B A M L   `   `   `  ����O  KwpfNumberPathTrainer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:wpfNumberPathTrainerwpfNumberPathTrainer  5          wpfNumberPathTrainer.App   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      4local"clr-namespace:wpfNumberPathTrainer     5        �� ShutdownMode$  OnExplicitShutdown=�5       �� 	Resources 5         CalculatorButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value  CalculatorComboBoxStyle
 ��Name ��Name �� SelectionBoxItem �� SelectionBoxItemTemplate d�Name �� 	ClickMode d� 	IsChecked �� Mode �� RelativeSource F�Name �Name � AllowsTransparency �� IsDropDownOpen �� MaxDropDownHeight ��Name J� SnapsToDevicePixels ��Name-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation  CalculatorComboBoxItemStyle �� Padding �� Triggers��%�  &	        &	 6    &	 �    �� 5   
   " ��6.   .�5      ��  � �� 5	      �� 5
      " ��6&   .R�5      �� #��z��6!   $ 25��5   !   $��0q�5   !   ._�5      �� $��Center=�6/   $��Center=�5   -   $��10q�5   -   5      5      5      �� 5       � ��
 �����6+   �� 5       � ��$ 16(�6)   �� 5       � ��$ 15,10q�6(   �� 5       � ��$ 50��6'   �� 5       � ��$ 50��6&    5      P� 5       �@J�IsMouseOver$
	 True��61   . �5      ��  �@J�Opacity$	 0.8Q�60   5      5      �� 5!   
   " ��60   .�5"      ��  � ��
 �����6+   �� 5#       � �� �#DDDDDD6,   �� 5$       � ��$ 1q�60   �� 5%       � ��$
 15,5q�6(   �� 5&       � ��$ 40��6'   �� 5'       � ��$ 16(�6)   �� 5(       � ��$ Center=�6;   �� 5)       � ��$ Center=�69   �� 5*       � �� 5+      �� 5,      " ��6&   .R�5-      � .+�5.      �� $
 Border��6%   #��z��5/   %   #��z��50   %   #��z��51   %   $ 20��52   %   ._�53   "   � .+�54   &   �� $ ContentPresenter��67   #��z 55   5   #��z 56   5   #��z��57   5   $��Center=�58   5   $��Center=�59   5   #��z��5:   5   d� 5;   &   $ ToggleButton��63   $ Press=�5=   1    5<   1   �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+��5>   *   �� 5?   .   " d�6>   .R�5@   2   �� $
 ToggleButtonBorder��69   
������ $��0q�5B   9   ._�5C   6   F� $ Arrow��6;   ���#666666$��Center=�5E   ;   $��Center=�5F   ;   "����        � 	=  	= � z     	5G   ;   5H   3   5I   /   5K   '   5L   #   5M      � 5N      $ 
PART_Popup��6$   $
 True��5O   $   #��z 5P   $   $��Bottom=�5Q   $   $��Slide=�5R   $   .��5S   "   �� $
 DropDownBorder��6)   
����������#DDDDDD$��1q�5V   )   $ 10��5W   )   #��z 5X   )   ._�5Y   &   �� $ DropDownScrollViewer��63   #��z 5Z   1   $
 True��5[   1   .��5\   *   �� $ ItemsPresenter��69   $ 	Contained=�5]   7   # z 5^   7   5_   '   5`   #   5a      5b      5c      5e       5f      P� 5g       �@J�IsMouseOver$
	 True��61   . �5h      ��  � �� �#2196F364   5i      5k      �� 5m   
   " ��64   .�5n      ��  � ��$ Center=�6;   �� 5o       � ��$ Center=�69   �� 5p       � ��$
 15,5q�6(   �� 5q       � ��$ 16(�6)   �� 5r       � �� 5s      �� 5t      " ��6&   .R�5u      �� $
 Border��6!   #��z��5v   !   #��z��5w   !   $��0q�5x   !   # z��5y   !   ._�5z      �� $��Center=�6/   $��Center=�5{   -   5|       5}      P� 5~       �@J�IsMouseOver$
	 True��6=   . �5   "   ��  � �� �#F5F5F56?   5�      P� 5�       �@��
IsSelected$
	 True��6<   . �5�   "   ��  � �� �#E3F2FD6?   5�      5�      5�      5�      5�      5�      