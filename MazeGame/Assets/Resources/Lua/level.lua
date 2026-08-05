print("Level script printed, Hi!")


	

function OnDialogue(lineId)
	local s = GetDialogueLine(1)
	SetIntroText(s)
	local e = GetDialogueLine(2)
	SetEndText(e)
end
