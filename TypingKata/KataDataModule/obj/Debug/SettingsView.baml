   M S B A M L   `   `   `  ����  KataDataModule!    KataDataModule.SettingsView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � ColumnDefinitions  5	   
   �� 5
      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   ��  � ToolTip$[ UTurn on learn mode. Gives guidance on what finger to press and does not save results.��6   $��0��5      $��0��6   $��Learning Mode��6-   
�������6E   d� 5   
   $[ UTurn on learn mode. Gives guidance on what finger to press and does not save results.��5      $��0��5      $��1��6%    d� 	IsChecked 5      �� *IsLearnModeOn+ �� Mode$ TwoWay=� � 	Resources 5       �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� Triggers P� Property P� Value��%�  'd�       '�� S    �� 5      " d�6   .�5      ��  � �� 5      �� 5       "	 d�6.   .R�5!   "   �� #��z��6)   #��z��5"   -   ._�5#   &   �� $��Center=�67   $��Center=�5$   ;   5%   #   5&      5(      
 5)      P� 5*       �@d�	IsChecked$
 TrueY�67   . �5+      ��  � �� �#FF80806;   �� 5,       � �� �#2727276;   �� 5-       � ��$ On��68   5.      P� 5/       �@d�	IsChecked$ FalseY�67   . �50      ��  � �� �#2727276;   �� 51       � ��
 �����6;   �� 52       � ��$	 Off��68   53      55      �� 56      " ��6   .�57      ��  �@��CornerRadius$ 10��65   �� 58       � �� �#FF808064   59      5:      5;      �� 5=   
    ^� ShowOnDisabled . $, &Resets all statistics and test results��5>      $��4��5?      $��0��6   $��0, 40, 0, 0q�5@      $��
Reset Data��5A      
����������#FF8080��6=   �� *ResetDataCommand+ 5B       �� CornerRadius��%�  '��       �� 5C      " ��6   .�5D      ��  � �� �#27272763   �� 5E       � �� 5F      �� 5G      "	 ��6.   .R�5H   "   � .+�5I   &   �� $ 6��6-   $��2q�6>   #��z��5J   -   #��z��5K   -   ._�5L   *   �� $��Center=�6;   $��Center=�6X   5M   '   5N   #   5O      5Q      
 5R      P� 5S       �@J�IsMouseOver . . �5T      ��  � �� �#FF80806;   5U      5W      5X      5Y      5Z      5[      