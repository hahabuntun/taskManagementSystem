export interface ITag {
    id: number;
    name: string;
    color: string;
}

export interface FullTagDTO {
    id: number;
    name: string;
    color: string;
}

export interface AddTagDTO {
    tagId: number;
}

export interface CreateTagDTO {
    name: string;
    color: string;
}

export interface AddTagsDTO {
    existingTagIds: number[];
    newTags: CreateTagDTO[];
}