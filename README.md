ML-Agents-Demo-1

installatie (https://github.com/Unity-Technologies/ml-agents/blob/release_6_docs/docs/Installation.md)

conda env list

conda create -n <<naam>> python=3.8.1
  
conda activate <<naam>>
  
pip install mlagents==0.19.0

mlagents-learn "<<config>>" --run-id=<<name>> (--force)
  
Ga naar folder waarin je results folder staat:
tensorboard --logdir results --port 6006

Interpretatie grafieken:
https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Using-Tensorboard.md


training configuratie file uitleg:
https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Training-ML-Agents.md#behavior-configurations

