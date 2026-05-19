print("Level script printed, Hi!")


	

function OnDialogue(lineId)
	local s = GetDialogueLine(lineId)
	SetIntroText(s)
end
